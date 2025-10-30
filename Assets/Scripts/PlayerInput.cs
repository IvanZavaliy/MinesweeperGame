using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Minefield minefield;
    bool isActive = true;

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            InteractWithCell();
        }
    }

    private void InteractWithCell()
    {
        RaycastHit2D hit = Utils.GetRaycastHit2DFromMousePosition();
        if (!hit) return;
        
        Vector2Int cellCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y);

        if (Input.GetMouseButtonDown(0))
            minefield.OpenCellByCoords(cellCoords);
        else
            minefield.SetBombFlag(cellCoords);
    }
}
