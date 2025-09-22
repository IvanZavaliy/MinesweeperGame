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

    [UnityEngine.Range(15, 50)]
    [SerializeField] private int bombPercentage;

    private List<Cell> cells = new List<Cell>();
    Dictionary<Vector2Int, Cell> positionToCell = new Dictionary<Vector2Int, Cell>();
    
    private int totalCells;
    private int bombsToSetup;
    private int remainedBomds;

    private int settedFlags = 0;
    private int closedCells;

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
    }

    public void StartGame()
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

        SetBombs();
    }

    private void SetBombs()
    {
        int setBombs = 0;
        while (setBombs < bombsToSetup)
        {
            int randomIndex = Random.Range(0, cells.Count);
            if (cells[randomIndex].IsBomb) continue;
            setBombs++;
        }
    }
}
