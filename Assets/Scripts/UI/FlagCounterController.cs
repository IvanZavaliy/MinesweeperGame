using UnityEngine;

namespace UI
{
    public class FlagCounterController : MonoBehaviour
    {
        [SerializeField] private Minefield minefield;
        [SerializeField] private GameCanvasView view;

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
}
