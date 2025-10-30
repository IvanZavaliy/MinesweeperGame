using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MinefieldVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject closedCell;
    [SerializeField] private Transform cellContainer;
    [SerializeField] private DigitSprites[]  digitSprites;
    [SerializeField] private Sprite flagSprite;
    [SerializeField] private Sprite closedSprite;

    public void VisualizeCellsOnStart(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            GameObject cellInstance = Instantiate(closedCell, new Vector2(cell.XCoord, cell.YCoord), Quaternion.identity);
            cellInstance.transform.parent = cellContainer;
            cell.CellInstance = cellInstance;
        }
    }

    public void OpenCell(Cell cell, int bombsAround)
    {
        cell.CellInstance.GetComponent<SpriteRenderer>().sprite = GetBombAroundSprite(bombsAround);
    }

    private Sprite GetBombAroundSprite(int bombsAround)
    {
        foreach (var sprite in digitSprites)
        {
            if (sprite.numberOfBombs == bombsAround) return sprite.digitSprite;
        }
        
        return null;
    }

    public void SetBombFlag(Cell cell, SetBombFlagResult result)
    {
        if (result == SetBombFlagResult.Setted)
            cell.CellInstance.GetComponent<SpriteRenderer>().sprite = flagSprite;
        else
            cell.CellInstance.GetComponent<SpriteRenderer>().sprite = closedSprite;
    }
}

[System.Serializable]
public class DigitSprites
{
    public int numberOfBombs;
    public Sprite digitSprite;
}