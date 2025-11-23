using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Minefield minefield;
    [SerializeField] private PlayerVisualizer playerVisualizer;
    public bool isActive = true;

    private Dictionary<KeyCode, Action<bool>> handleActions;
    private KeyCode currentHandleActiveKey = KeyCode.None;
    
    public static event Action OnMoveUp;
    public static event Action OnMoveDown;
    public static event Action OnMoveRight;
    public static event Action OnMoveLeft;
    public static event Action<KeyCode> OnDigUpCell;

    private void Awake()
    {
        handleActions = new Dictionary<KeyCode, Action<bool>>
        {
            { KeyCode.RightArrow, playerVisualizer.SetActiveRightPlayerHandle },
            { KeyCode.LeftArrow, playerVisualizer.SetActiveLeftPlayerHandle },
            { KeyCode.DownArrow, playerVisualizer.SetActiveDownPlayerHandle },
            { KeyCode.UpArrow, playerVisualizer.SetActiveUpPlayerHandle }
        };
    }

    private void Update()
    {
        if (!isActive) return;

        // Player spawn and dig starting zone
        if (Input.GetMouseButtonUp(0) && !minefield.GameStarted)
        {
            DigUpStartingCell();
        }
        
        // PLayer movement
        PlayerMovementInput();
        
        // Player interaction
        PlayerDiggingInteraction();
    }

    private void DigUpStartingCell()
    {
        RaycastHit2D hit = Utils.GetRaycastHit2DFromMousePosition();
        if (!hit) return;
        Vector2Int coords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y); 
        
        minefield.OpenCellByCoords(coords);
        minefield.GetCoordsForPlayer(coords);
    }
    
    private void PlayerMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnMoveUp?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnMoveDown?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnMoveRight?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnMoveLeft?.Invoke();
        }
    }

    private void PlayerDiggingInteraction()
    {
        if (currentHandleActiveKey == KeyCode.None)
        {
            foreach (var key in handleActions.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    currentHandleActiveKey = key;
                    handleActions[key].Invoke(true);
                    break;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                print("dig");
                SetInactiveHandle();
                OnDigUpCell?.Invoke(currentHandleActiveKey);
            }
            else if (Input.GetKeyUp(currentHandleActiveKey))
            {
                SetInactiveHandle();
                currentHandleActiveKey = KeyCode.None;
            }
        }

        void SetInactiveHandle()
        {
            if (handleActions.ContainsKey(currentHandleActiveKey))
            {
                handleActions[currentHandleActiveKey].Invoke(false);
            }
        }
    }
}
