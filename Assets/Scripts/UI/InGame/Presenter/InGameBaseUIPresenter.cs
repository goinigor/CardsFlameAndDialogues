using System;
using CFD.Core;

namespace CFD.UI.InGame
{
    public class InGameBaseUIPresenter : IDisposable
    {
        private readonly InGameBaseUI _inGameBaseUI;
        private readonly ISceneController _sceneController;

        public InGameBaseUIPresenter(InGameBaseUI inGameBaseUI, ISceneController sceneController)
        {
            _inGameBaseUI = inGameBaseUI;
            _sceneController = sceneController;
        }
        
        public void Initialize()
        {
            _inGameBaseUI.OnBackToMenuButtonClicked += OnBackToMenuButtonClicked;
        }

        private void OnBackToMenuButtonClicked()
        {
            _sceneController.LoadScene(1);
        }

        public void Dispose()
        {
            _inGameBaseUI.OnBackToMenuButtonClicked -= OnBackToMenuButtonClicked;
        }
    }
}