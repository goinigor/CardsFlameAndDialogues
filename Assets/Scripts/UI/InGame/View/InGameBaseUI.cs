using System;
using CFD.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CFD.UI.InGame
{
    public class InGameBaseUI : View
    {
        public event Action OnBackToMenuButtonClicked;
        
        [SerializeField] private Button _backToMenuButton;

        private void Awake()
        {
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
        }

        private void OnBackToMenuButtonClick()
        {
            OnBackToMenuButtonClicked?.Invoke();
        }
    }
}