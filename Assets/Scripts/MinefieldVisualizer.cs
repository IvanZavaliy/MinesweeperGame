using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MinefieldVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject closedCell;
    [SerializeField] private Transform cellContainer;

    public void VisualizeCellsOnStart(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            GameObject cellInstance = Instantiate(closedCell, new Vector2(cell.XCoord, cell.YCoord), Quaternion.identity);
            cellInstance.transform.parent = cellContainer;
            cell.CellInstance = cellInstance;
        }
    }
}
