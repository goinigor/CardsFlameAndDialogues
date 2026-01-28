using System;
using CFD.Core;

namespace CFD.UI.Menu
{
    public class MainMenuPresenter : IDisposable
    {
        private readonly MainMenuView _mainMenuView;
        private readonly ISceneController _sceneController;

        public MainMenuPresenter(MainMenuView mainMenuView, ISceneController sceneController)
        {
            _mainMenuView = mainMenuView;
            _sceneController = sceneController;
        }

        public void Initialize()
        {
            _mainMenuView.OnSceneSelected += OnSceneSelected;
        }

        private void OnSceneSelected(int buildIndex)
        {
            _sceneController.LoadScene(buildIndex);
        }

        public void Dispose()
        {
            _mainMenuView.OnSceneSelected -= OnSceneSelected;
        }
    }
}