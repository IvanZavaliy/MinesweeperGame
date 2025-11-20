using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        if (!isMoving) StartCoroutine(MoveStep(Vector3.up * stepDistance));
    }
    void HandleMoveDown()
    {
        if (!isMoving) StartCoroutine(MoveStep(Vector3.down * stepDistance));
    }
    void HandleMoveRight()
    {
        if (!isMoving) StartCoroutine(MoveStep(Vector3.right * stepDistance));
    }
    void HandleMoveLeft()
    {
        if (!isMoving) StartCoroutine(MoveStep(Vector3.left * stepDistance));
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
        
        transform.position = endPos; 
        isMoving = false;
    }
}
