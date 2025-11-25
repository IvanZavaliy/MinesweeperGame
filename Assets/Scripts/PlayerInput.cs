using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Minefield minefield;
    [SerializeField] private PlayerVisualizer playerVisualizer;
    [SerializeField] private PlayerLogic playerLogic;
    public bool isActive = true;

    private KeyCode[] handlesInputKeys =
    {
        KeyCode.RightArrow, KeyCode.LeftArrow,
        KeyCode.UpArrow, KeyCode.DownArrow,
    };
    private KeyCode currentHandleActiveKey = KeyCode.None;
    
    public static event Action OnMoveUp;
    public static event Action OnMoveDown;
    public static event Action OnMoveRight;
    public static event Action OnMoveLeft;
    public static event Action<KeyCode> OnDigUpCell;
    public static event Action<KeyCode> OnFlaggedCell;

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
            foreach (var key in handlesInputKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    currentHandleActiveKey = key;
                    playerVisualizer.SetActivePlayerHandle(true,  currentHandleActiveKey);
                    break;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && 
                playerLogic.HandleInMinefieldCheck[currentHandleActiveKey].Invoke(currentHandleActiveKey))
            {
                print("dig");
                SetInactiveHandle();
                OnDigUpCell?.Invoke(currentHandleActiveKey);
            }
            if (Input.GetKeyDown(KeyCode.F) &&
                playerLogic.HandleInMinefieldCheck[currentHandleActiveKey].Invoke(currentHandleActiveKey))
            {
                print("flag setted");
                SetInactiveHandle();
                OnFlaggedCell?.Invoke(currentHandleActiveKey);
            }
            if (Input.GetKeyUp(currentHandleActiveKey))
            {
                SetInactiveHandle();
                currentHandleActiveKey = KeyCode.None;
            }
        }

        void SetInactiveHandle()
        {
            if (handlesInputKeys.Contains(currentHandleActiveKey))
            {
                playerVisualizer.SetActivePlayerHandle(false, currentHandleActiveKey);
            }
        }
    }
}
