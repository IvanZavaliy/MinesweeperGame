using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MenuUIController : MonoBehaviour
    {
        [SerializeField] private MenuCanvasView view;
        
        [SerializeField] private GameObject leaderboardContainer;
        [SerializeField] private DatabaseManager dbManager;
        
        private bool _isLeaderboardOpenedFirst = false;
        
        private Dictionary<int, Action> _dropdownActions;

        private void Awake()
        {
            _dropdownActions = new Dictionary<int, Action>
            {
                {0, () => dbManager.UpdateLeaderboard(Difficulty.Easy)},
                {1, () => dbManager.UpdateLeaderboard(Difficulty.Medium)},
                {2, () => dbManager.UpdateLeaderboard(Difficulty.Hard)}
            };
        }

        public void LeaderboardContainerSetActive(bool active)
        {
            leaderboardContainer.SetActive(active);
            if (leaderboardContainer.activeSelf && !_isLeaderboardOpenedFirst)
            {
                dbManager.UpdateLeaderboard(Difficulty.Easy);
                _isLeaderboardOpenedFirst = true;
            }
        }

        public void LeaderboardDropdownAction(int index)
        {
            _dropdownActions[index]?.Invoke();
        }
    }
}
