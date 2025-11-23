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
    
    private Dictionary<KeyCode, Vector2Int> handleDirections;

    private void Awake()
    {
        handleDirections = new Dictionary<KeyCode, Vector2Int>
        {
            { KeyCode.RightArrow, new Vector2Int(1, 0) },
            { KeyCode.LeftArrow, new Vector2Int(-1, 0) },
            { KeyCode.DownArrow, new Vector2Int(0, -1) },
            { KeyCode.UpArrow, new Vector2Int(0, 1) }
        };
    }

    private void OnEnable()
    {
        PlayerInput.OnMoveUp += PlayerMoveUp;
        PlayerInput.OnMoveDown += PlayerMoveDown;
        PlayerInput.OnMoveRight += PlayerMoveRight;
        PlayerInput.OnMoveLeft += PlayerMoveLeft;
        PlayerInput.OnDigUpCell += DigUpCell;
    }

    private void OnDisable()
    {
        PlayerInput.OnMoveUp -= PlayerMoveUp;
        PlayerInput.OnMoveDown -= PlayerMoveDown;
        PlayerInput.OnMoveRight -= PlayerMoveRight;
        PlayerInput.OnMoveLeft -= PlayerMoveLeft;
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
        RaycastHit2D hit = Utils.GetRaycastHit2DFromWorldObjectPosition(transform.position);
        Vector2Int coords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y);
        
        minefield.CellCollisionCheck(coords);
    }
    
    private void DigUpCell(KeyCode currentHandleActiveKey)
    {
        int xHandleCoord = handleDirections[currentHandleActiveKey].x;
        int yHandleCoord = handleDirections[currentHandleActiveKey].y;
        
        RaycastHit2D hit = Utils.GetRaycastHit2DFromWorldObjectPosition(transform.position);
        Vector2Int coords = new Vector2Int((int)hit.transform.position.x + xHandleCoord, (int)hit.transform.position.y + yHandleCoord);
        
        minefield.OpenCellByCoords(coords);
    }

    private PlayerPositionCheck IsPlayerInMinefield()
    {
        return new PlayerPositionCheck(minefield, transform.position);
    }
}

class PlayerPositionCheck
{
    private readonly Minefield _minefield;
    private readonly Vector3 _playerPosition;

    public PlayerPositionCheck(Minefield minefield, Vector3 playerPosition)
    {
        _minefield = minefield;
        _playerPosition = playerPosition;
    }

    public bool IsLeft()
    {
        return _minefield.MinefieldBordersCoords[0].x < _playerPosition.x;
    }

    public bool IsRight()
    {
        return _minefield.MinefieldBordersCoords[1].x > _playerPosition.x;
    }

    public bool IsUp()
    {
        return  _minefield.MinefieldBordersCoords[1].y > _playerPosition.y;
    }
    
    public bool IsDown()
    {
        return _minefield.MinefieldBordersCoords[0].y < _playerPosition.y;
    }
}