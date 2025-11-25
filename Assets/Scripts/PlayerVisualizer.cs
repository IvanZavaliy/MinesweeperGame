using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject rightPlayerHandle;
    [SerializeField] private GameObject leftPlayerHandle;
    [SerializeField] private GameObject upPlayerHandle;
    [SerializeField] private GameObject downPlayerHandle;
    private Dictionary<KeyCode, GameObject> HandlesInstances;

    private void Awake()
    {
        HandlesInstances = new Dictionary<KeyCode, GameObject>
        {
            { KeyCode.RightArrow, rightPlayerHandle },
            { KeyCode.LeftArrow, leftPlayerHandle },
            { KeyCode.DownArrow, downPlayerHandle },
            { KeyCode.UpArrow, upPlayerHandle },
        };
    }

    public void SetActivePlayerHandle(bool active, KeyCode handleDirection)
    {
        HandlesInstances[handleDirection].SetActive(active);
    }
}
