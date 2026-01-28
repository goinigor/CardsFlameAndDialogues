using CFD.Core;
using UnityEngine;

namespace CFD.UI.InGame
{
    public class AceOfShadowsInstaller : AbstractMonoInstaller
    {
        [SerializeField] private InGameBaseUI _inGameBaseUI;
        private InGameBaseUIPresenter _inGameBaseUIPresenter;
        
        public override void InstallBindings()
        {
            var sceneController = ServiceLocator.Resolve<ISceneController>();
            _inGameBaseUIPresenter = new InGameBaseUIPresenter(_inGameBaseUI, sceneController);
        }

        public override void Initialize()
        {
            _inGameBaseUIPresenter.Initialize();
        }

        public override void Dispose()
        {
            _inGameBaseUIPresenter.Dispose();
        }
    }
}