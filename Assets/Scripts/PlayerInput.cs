using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Minefield minefield;
    [SerializeField] private Player player;
    public bool isActive = true;

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            InteractWithCell();
        }
    }

    private void InteractWithCell()
    {
        RaycastHit2D hit = Utils.GetRaycastHit2DFromMousePosition();
        if (!hit) return;
        
        Vector2Int coords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y);

        if (Input.GetMouseButtonUp(0))
        {
            if (!minefield.GameStarted) minefield.GetCoordsForPlayer(coords);
            minefield.OpenCellByCoords(coords);
        }
        else if  (Input.GetMouseButtonUp(1) && minefield.GameStarted)
            minefield.SetBombFlag(coords);
    }
}
