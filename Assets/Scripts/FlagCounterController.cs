using System;
using UnityEngine;

public class FlagCounterController : MonoBehaviour
{
    [SerializeField] private Minefield minefield;
    [SerializeField] private CanvasView view;

    private void Awake()
    {
        minefield.UpdateSettedFlags += flagsCount =>
        {
            view.UpdateFlagCounterToDisplay(minefield.BombsToSetup - flagsCount);
        };
    }

    void Start()
    {
        view.UpdateFlagCounterToDisplay(minefield.BombsToSetup);
    }
}
