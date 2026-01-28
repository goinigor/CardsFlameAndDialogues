using CFD.UI.Menu;
using UnityEngine;

namespace CFD.Core
{
    /// <summary>
    /// Similar to the Bootstrap class, but for the current scene context. Initialize and resolve dependencies
    /// </summary>
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private MainMenuView _mainMenuView;
        
        private MainMenuPresenter _mainMenuPresenter;

        private void Awake()
        {
            var sceneController = ServiceLocator.Resolve<ISceneController>();
            
            _mainMenuPresenter = new MainMenuPresenter(_mainMenuView, sceneController);
        }

        private void Start()
        {
            _mainMenuPresenter.Initialize();
        }

        private void OnDestroy()
        {
            _mainMenuPresenter.Dispose();
        }
    }
}