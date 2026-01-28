using System;
using System.Collections.Generic;
using CFD.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CFD.UI.Menu
{
    public class MainMenuView : View
    {
        public event Action<int> OnSceneSelected;
        
        [SerializeField] private List<MenuSceneButton> _sceneButtons;

        private void Awake()
        {
            foreach (var menuSceneButton in _sceneButtons)
            {
                menuSceneButton.OnButtonClicked += OnMenuSceneButtonClicked;
            }
        }

        private void OnMenuSceneButtonClicked(int buildIndex)
        {
            OnSceneSelected?.Invoke(buildIndex);
        }
    }
}