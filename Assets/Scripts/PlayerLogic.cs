using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] Minefield minefield;
    
    [SerializeField] private float stepDistance = 1f;
    [SerializeField] private float moveDuration = 0.1f;
    private bool isMoving = false;
    
    private Dictionary<KeyCode, Vector2Int> handleDirections = new Dictionary<KeyCode, Vector2Int>
    {
        { KeyCode.RightArrow, new Vector2Int(1, 0) },
        { KeyCode.LeftArrow, new Vector2Int(-1, 0) },
        { KeyCode.DownArrow, new Vector2Int(0, -1) },
        { KeyCode.UpArrow, new Vector2Int(0, 1) }
    };
    private Dictionary<KeyCode, Func<KeyCode, bool>> handleInMinefieldCheck;
    
    public Dictionary<KeyCode, Func<KeyCode, bool>> HandleInMinefieldCheck => handleInMinefieldCheck;

    private void Awake()
    {
        handleInMinefieldCheck = new Dictionary<KeyCode, Func<KeyCode, bool>>
        {
            { KeyCode.RightArrow, keyCode => IsPlayerInMinefield(keyCode).IsRight() },
            { KeyCode.LeftArrow, keyCode => IsPlayerInMinefield(keyCode).IsLeft() },
            { KeyCode.DownArrow, keyCode => IsPlayerInMinefield(keyCode).IsDown() },
            { KeyCode.UpArrow, keyCode => IsPlayerInMinefield(keyCode).IsDown() }
        };
    }

    private void OnEnable()
    {
        PlayerInput.OnMoveUp += PlayerMoveUp;
        PlayerInput.OnMoveDown += PlayerMoveDown;
        PlayerInput.OnMoveRight += PlayerMoveRight;
        PlayerInput.OnMoveLeft += PlayerMoveLeft;
        PlayerInput.OnDigUpCell += DigUpCell;
        PlayerInput.OnFlaggedCell += SetBombFlagOnCell;
    }

    private void OnDisable()
    {
        PlayerInput.OnMoveUp -= PlayerMoveUp;
        PlayerInput.OnMoveDown -= PlayerMoveDown;
        PlayerInput.OnMoveRight -= PlayerMoveRight;
        PlayerInput.OnMoveLeft -= PlayerMoveLeft;
        PlayerInput.OnDigUpCell -= DigUpCell;
        PlayerInput.OnFlaggedCell -= SetBombFlagOnCell;
    }

    private void PlayerMoveUp()
    {
        if (!isMoving && IsPlayerInMinefield().IsUp())
            StartCoroutine(MoveStep(Vector3.up * stepDistance));
    }
    private void PlayerMoveDown()
    {
        if (!isMoving && IsPlayerInMinefield().IsDown())
            StartCoroutine(MoveStep(Vector3.down * stepDistance));
    }
    private void PlayerMoveRight()
    {
        if (!isMoving && IsPlayerInMinefield().IsRight())
            StartCoroutine(MoveStep(Vector3.right * stepDistance));
    }
    private void PlayerMoveLeft()
    {
        if (!isMoving && IsPlayerInMinefield().IsLeft())
            StartCoroutine(MoveStep(Vector3.left * stepDistance));
    }

    IEnumerator MoveStep(Vector3 moveVector)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + moveVector;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        PlayerRaycast();
        transform.position = endPos; 
        isMoving = false;
    }

    private void PlayerRaycast()
    {
        Vector2Int coords = GetCoordsOfInteractedCell();
        minefield.CellCollisionCheck(coords);
    }
    
    private void DigUpCell(KeyCode currentHandleActiveKey)
    {
        Vector2Int coords = GetCoordsOfInteractedCell(currentHandleActiveKey);
        minefield.OpenCellByCoords(coords);
    }
    
    private void SetBombFlagOnCell(KeyCode currentHandleActiveKey)
    {
        Vector2Int coords = GetCoordsOfInteractedCell(currentHandleActiveKey);
        minefield.SetBombFlag(coords);
    }

    private Vector2Int GetCoordsOfInteractedCell(KeyCode currentHandleActiveKey = KeyCode.None)
    {
        int xCoord = 0, yCoord = 0;

        if (currentHandleActiveKey != KeyCode.None)
        {
            xCoord = handleDirections[currentHandleActiveKey].x;
            yCoord = handleDirections[currentHandleActiveKey].y;
        }
        
        RaycastHit2D hit = Utils.GetRaycastHit2DFromWorldObjectPosition(transform.position);
        Vector2Int coords = new Vector2Int((int)hit.transform.position.x + xCoord, (int)hit.transform.position.y + yCoord);
        
        return coords;
    }
    
    private PlayerPositionCheck IsPlayerInMinefield(KeyCode currentHandleActiveKey = KeyCode.None)
    {
        return new PlayerPositionCheck(minefield, transform.position, handleDirections, currentHandleActiveKey);
    }
}

class PlayerPositionCheck
{
    private readonly Minefield _minefield;
    private readonly Vector3 _playerPosition;
    
    private  readonly Dictionary<KeyCode, Vector2Int> _handleDirections;
    private readonly KeyCode _currentHandleActiveKey;
    private readonly int _handleXCoord = 0;
    private readonly int _handleYCoord = 0;

    public PlayerPositionCheck(Minefield minefield, Vector3 playerPosition, 
        Dictionary<KeyCode, Vector2Int> handleDirections, KeyCode currentHandleActiveKey)
    {
        _minefield = minefield;
        _playerPosition = playerPosition;
        
        _handleDirections = handleDirections;
        _currentHandleActiveKey = currentHandleActiveKey;
        if (_currentHandleActiveKey != KeyCode.None)
        {
            _handleXCoord = handleDirections[_currentHandleActiveKey].x;
            _handleYCoord = handleDirections[_currentHandleActiveKey].y;
        }
    }

    public bool IsLeft()
    {
        return _minefield.MinefieldBordersCoords[0].x < _playerPosition.x + _handleXCoord;
    }

    public bool IsRight()
    {
        return _minefield.MinefieldBordersCoords[1].x > _playerPosition.x + _handleXCoord;
    }

    public bool IsUp()
    {
        return  _minefield.MinefieldBordersCoords[1].y > _playerPosition.y + _handleYCoord;
    }
    
    public bool IsDown()
    {
        return _minefield.MinefieldBordersCoords[0].y < _playerPosition.y + _handleYCoord;
    }
}