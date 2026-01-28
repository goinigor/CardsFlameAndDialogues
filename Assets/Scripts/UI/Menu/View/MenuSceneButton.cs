using System;
using CFD.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CFD.UI.Menu
{
    /// <summary>
    /// Button for loading a scene
    /// </summary>
    public class MenuSceneButton : View
    {
        public event Action<int> OnButtonClicked;
        
        [SerializeField] private Button _button;
        [SerializeField] private int _sceneIndex;

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected override void VirtualOnDestroy()
        {
            base.VirtualOnDestroy();
            _button.onClick.RemoveListener(OnClick);
            OnButtonClicked = null;
        }

        private void OnClick()
        {
            OnButtonClicked?.Invoke(_sceneIndex);
        }
    }
}