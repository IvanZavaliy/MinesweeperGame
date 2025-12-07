using System;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Minefield : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] MinefieldVisualizer visualizer;
    [SerializeField] PlayerInput playerInput;

    [UnityEngine.Range(10, 50)]
    [SerializeField] private int bombPercentage;
    
    [SerializeField] private GameCanvasView gameCanvasView;
    [SerializeField] private TimerController timerController;
    
    private List<Cell> cells = new List<Cell>();
    private Dictionary<Vector2Int, Cell> positionToCell = new Dictionary<Vector2Int, Cell>();
    private List<Vector2Int> minefieldBordersCoords = new List<Vector2Int>();
    
    private bool isGameStarted = false;
    private int totalCells; // Загальна кількість клітинок
    private int bombsToSetup; // Кількість замінованих клітинок
    private int remainedBomds; // Кількість закритих замінованих клітинок

    private int settedFlags = 0; // Кількість встановлених прапорців
    public Action<int> UpdateSettedFlags;
    
    private int closedCells; // Кількість закритих клітинок
    
    public int Width => width;
    public int Height => height;
    public bool GameStarted => isGameStarted;
    public int BombsToSetup => bombsToSetup;

    private int SettedFlags
    {
        get => settedFlags;
        set
        {
            if (settedFlags == value) return;
            
            settedFlags = value;
            UpdateSettedFlags?.Invoke(settedFlags);
        }
    }
    public List<Vector2Int> MinefieldBordersCoords => minefieldBordersCoords;

    private void Awake()
    {
        totalCells = width * height;
        bombsToSetup = totalCells * bombPercentage / 100;
        remainedBomds = bombsToSetup;
        closedCells = totalCells;
    }

    private void Start()
    {
        StartGame();
        
        minefieldBordersCoords.Add(new Vector2Int(cells[0].XCoord, cells[0].YCoord));
        minefieldBordersCoords.Add(new Vector2Int(cells[^1].XCoord, cells[^1].YCoord));
    }

    private void StartGame()
    {
        CreateMinefield();
        visualizer.VisualizeCellsOnStart(cells);
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
            SettedFlags++;
            visualizer.SetBombFlag(cell, result);
            
            if (cell.IsBomb) remainedBomds--;
        }

        if (result == SetBombFlagResult.Unsetted)
        {
            SettedFlags--;
            visualizer.SetBombFlag(cell, result);
            
            if (cell.IsBomb)
                remainedBomds++;
        }
    }
    
    public void CellCollisionCheck(Vector2Int playerCoords)
    {
        Cell cell = positionToCell[playerCoords];
        CellStatusResult statusResult = cell.CellStatus();

        if (statusResult == CellStatusResult.Opened || cell.IsFlagged) return;

        if (cell.IsBomb)
        {
            LoseLogic();
        }
    }
    
    public void OpenCellByCoords(Vector2Int cellCoords)
    {
        Cell cell = positionToCell[cellCoords];
        
        if (!isGameStarted)
        {
            isGameStarted = true;
            SetBombs(cell);
            timerController.StartTimer();
        }
        
        CellStatusResult statusResult = cell.CellStatus();
        if (statusResult == CellStatusResult.Closed)
        {
            cell.IsOpened = true;
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

        if (statusResult == CellStatusResult.GameOver)
        {
            LoseLogic();
        }

        if (closedCells == bombsToSetup)
        {
            WinLogic();
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

    private void WinLogic()
    {
        print("You win");
        visualizer.SetFlagsOnWin(cells);
        SettedFlags = BombsToSetup;
        playerInput.isActive = false;
        gameCanvasView.ShowWinMenu();
        timerController.StopTimer();
    }

    private void LoseLogic()
    {
        print("You lose");
        visualizer.BombVisualize(cells);
        playerInput.isActive = false;
        gameCanvasView.ShowLoseMenu();
        timerController.StopTimer();
    }

    public void GetCoordsForPlayer(Vector2Int coords)
    {
        Cell cellSpawnPoint = positionToCell[coords];
        visualizer.SpawnPlayer(cellSpawnPoint.XCoord, cellSpawnPoint.YCoord);
    }
}
