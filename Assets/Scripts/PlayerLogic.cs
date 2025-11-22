using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] Minefield minefield;
    
    public float stepDistance = 1f;
    public float moveDuration = 0.25f;
    private bool isMoving = false;

    void OnEnable()
    {
        PlayerInput.OnMoveUp += HandleMoveUp;
        PlayerInput.OnMoveDown += HandleMoveDown;
        PlayerInput.OnMoveRight += HandleMoveRight;
        PlayerInput.OnMoveLeft += HandleMoveLeft;
    }

    void OnDisable()
    {
        PlayerInput.OnMoveUp -= HandleMoveUp;
        PlayerInput.OnMoveDown -= HandleMoveDown;
        PlayerInput.OnMoveRight -= HandleMoveRight;
        PlayerInput.OnMoveLeft -= HandleMoveLeft;
    }

    void HandleMoveUp()
    {
        if (!isMoving && IsPlayerInMinefield().IsUp())
            StartCoroutine(MoveStep(Vector3.up * stepDistance));
    }
    void HandleMoveDown()
    {
        if (!isMoving && IsPlayerInMinefield().IsDown())
            StartCoroutine(MoveStep(Vector3.down * stepDistance));
    }
    void HandleMoveRight()
    {
        if (!isMoving && IsPlayerInMinefield().IsRight())
            StartCoroutine(MoveStep(Vector3.right * stepDistance));
    }
    void HandleMoveLeft()
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