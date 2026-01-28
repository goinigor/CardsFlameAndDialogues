using CFD.UI.Menu;
using UnityEngine;

namespace CFD.Core
{
    public class MenuSceneInstaller : AbstractMonoInstaller
    {
        [SerializeField] private MainMenuView _mainMenuView;
        
        private MainMenuPresenter _mainMenuPresenter;

        public override void InstallBindings()
        {
            
            var sceneController = ServiceLocator.Resolve<ISceneController>();
            
            _mainMenuPresenter = new MainMenuPresenter(_mainMenuView, sceneController);
        }
        
        public override void Initialize()
        {
            _mainMenuPresenter.Initialize();
        }
        
        public override void Dispose()
        {
            _mainMenuPresenter.Dispose();
        }
    }
}