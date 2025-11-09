using System;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class Minefield : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] MinefieldVisualizer visualizer;
    [SerializeField] PlayerInput playerInput;

    [UnityEngine.Range(15, 50)]
    [SerializeField] private int bombPercentage;

    [SerializeField] private GameObject winMenuPopUp;
    [SerializeField] private GameObject loseMenuPopUp;
    private List<Cell> cells = new List<Cell>();
    Dictionary<Vector2Int, Cell> positionToCell = new Dictionary<Vector2Int, Cell>();
    
    private bool isGameStarted = false;
    private int totalCells; // Загальна кількість клітинок
    private int bombsToSetup; // Кількість замінованих клітинок
    private int remainedBomds; // Кількість закритих замінованих клітинок

    private int settedFlags = 0; // Кількість встановлених прапорців
    private int closedCells; // Кількість закритих клітинок
    
    public int Width { get => width; }
    public int Height { get => height; }

    private void Awake()
    {
        Screen.SetResolution(1080, 720, false);
        totalCells = width * height;
        bombsToSetup = totalCells * bombPercentage / 100;
        remainedBomds = bombsToSetup;
        closedCells = totalCells;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CreateMinefield();
        visualizer.VisualizeCellsOnStart(cells);
        //OpenRandomEmptyCell();
    }
    
    private void OpenRandomEmptyCell()
    {
        bool isOpened = false;
        List<Cell> cellsToChooseFrom = new List<Cell>(cells);

        while (!isOpened && cellsToChooseFrom.Count > 0)
        {
            int randomIndex = Random.Range(0, cellsToChooseFrom.Count);
            Cell cell = cellsToChooseFrom[randomIndex];
            if (cell.IsBomb || GetBombsAroundCell(cell) != 0)
            {
                cellsToChooseFrom.Remove(cell);
                continue;
            }
            OpenCellByCoords(new Vector2Int(cell.XCoord, cell.YCoord));
            isOpened = true;
        }
    }

    private void CreateMinefield()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = new Cell(i, j);
                cells.Add(cell);
                positionToCell[new Vector2Int(i, j)] = cell;
            }
        }
    }

    private void SetBombs(Cell cell)
    {
        int setBombs = 0;
        while (setBombs < bombsToSetup)
        {
            int randomIndex = Random.Range(0, cells.Count);
            if (cells[randomIndex].IsBomb) continue;

            bool isInSafeArea = Math.Abs(cells[randomIndex].XCoord - cell.XCoord) <= 1 &&
                                Math.Abs(cells[randomIndex].YCoord - cell.YCoord) <= 1;

            if (!isInSafeArea)
            {
                cells[randomIndex].IsBomb = true;
                setBombs++;
            }
        }
    }

    public void SetBombFlag(Vector2Int cellCoords)
    {
        Cell cell = positionToCell[cellCoords];
        SetBombFlagResult result = cell.SetBombFlag();

        if (result == SetBombFlagResult.Setted)
        {
            settedFlags++;
            visualizer.SetBombFlag(cell, result);
            
            if (cell.IsBomb)
            {
                remainedBomds--;
                if (remainedBomds == 0 && settedFlags == bombsToSetup)
                {
                    print("You win"); // Заглушка
                    playerInput.isActive = false;
                    winMenuPopUp.SetActive(true);
                }
            }
        }

        if (result == SetBombFlagResult.Unsetted)
        {
            settedFlags--;
            visualizer.SetBombFlag(cell, result);
            
            if (cell.IsBomb)
                remainedBomds++;
        }
    }
    
    public void OpenCellByCoords(Vector2Int cellCoords)
    {
        Cell cell = positionToCell[cellCoords];
        
        if (!isGameStarted)
        {
            isGameStarted = true;
            SetBombs(cell);
        }
        
        OpenCellResult result = cell.OpenCell();
        if (result == OpenCellResult.Opened)
        {
            int bombsAround = GetBombsAroundCell(cell);
            visualizer.OpenCell(cell, bombsAround);
            closedCells--;

            if (bombsAround == 0)
            {
                foreach (Cell neighbour in GetNeighbourCells(cell))
                {
                    OpenCellByCoords(new Vector2Int(neighbour.XCoord, neighbour.YCoord));
                }
            }
        }

        if (result == OpenCellResult.GameOver)
        {
            print("You lose"); // Заглушка
            visualizer.BombVisualize(cells);
            playerInput.isActive = false;
            loseMenuPopUp.SetActive(true);
        }

        if (closedCells == bombsToSetup)
        {
            print("You win"); // Заглушка
            visualizer.SetFlagsOnWin(cells);
            playerInput.isActive = false;
            winMenuPopUp.SetActive(true);
        }
    }
    
    private int GetBombsAroundCell(Cell cell)
    {
        int bombsAround = 0;
        foreach (var neighbour in GetNeighbourCells(cell))
        {
            if (neighbour.IsBomb)
                bombsAround++;
        }

        return bombsAround;
    }

    private IEnumerable<Cell> GetNeighbourCells(Cell cell)
    {
        List<Cell> neighbourCells = new List<Cell>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2Int neighbourCoords = new Vector2Int(cell.XCoord + i, cell.YCoord + j);
                if (!positionToCell.ContainsKey(neighbourCoords) || (i == 0 && j == 0)) continue;
                Cell neighbourCell = positionToCell[neighbourCoords];
                
                neighbourCells.Add(neighbourCell);
            }
        }
        
        return neighbourCells;
    }
}
