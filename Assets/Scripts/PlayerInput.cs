using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Minefield minefield;
    [SerializeField] private Player player;
    public bool isActive = true;
    
    public static event Action OnMoveUp;
    public static event Action OnMoveDown;
    public static event Action OnMoveRight;
    public static event Action OnMoveLeft;

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetMouseButtonUp(0) && !minefield.GameStarted)
        {
            DigUpStartingCell();
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnMoveUp?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            OnMoveDown?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            OnMoveRight?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            OnMoveLeft?.Invoke();
        }
    }

    private void DigUpStartingCell()
    {
        RaycastHit2D hit = Utils.GetRaycastHit2DFromMousePosition();
        if (!hit) return;
        
        Vector2Int coords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y); 
        
        minefield.GetCoordsForPlayer(coords);
        minefield.OpenCellByCoords(coords);
    }
}
