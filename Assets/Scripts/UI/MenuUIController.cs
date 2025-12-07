using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MenuUIController : MonoBehaviour
    {
        [SerializeField] private MenuCanvasView view;
        
        [SerializeField] private GameObject leaderboardContainer;
        
        private Dictionary<int, Action<int>> _dropdownActions;

        private void Awake()
        {
            _dropdownActions = new Dictionary<int, Action<int>>
            {
                {0, VisualizeLeaderboard},
                {1, VisualizeLeaderboard},
                {2, VisualizeLeaderboard},
            };
        }

        public void LeaderboardContainerSetActive(bool active)
        {
            leaderboardContainer.SetActive(active);
        }

        public void LeaderboardDropdownAction(int index)
        {
            _dropdownActions[index]?.Invoke(index);
        }
        
        private void VisualizeLeaderboard(int index)
        {
            view.ShowLeaderboard(index);
        }
    }
}
