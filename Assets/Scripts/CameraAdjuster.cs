using System;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] Minefield minefield;

    private void Awake()
    {
        Screen.SetResolution(1080, 720, false);
        
        GetComponent<Camera>().orthographicSize = minefield.Height / 2f + 2.5f;
        transform.position = new Vector3(minefield.Width / 2f - 0.5f, minefield.Height / 2f - 0.5f, -10);
    }
}
