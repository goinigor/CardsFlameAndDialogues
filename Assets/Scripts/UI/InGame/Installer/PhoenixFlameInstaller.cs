using CFD.Core;
using CFD.Features.PhoenixFlame;
using CFD.Features.PhoenixFlame.Presenter;
using UnityEngine;

namespace CFD.UI.InGame
{
    public class PhoenixFlameInstaller : AbstractMonoInstaller
    {
        [SerializeField] private InGameBaseUI _inGameBaseUI;
        [SerializeField] private FlameColorControllerView _flameColorControllerView;
        [SerializeField] private FlameObjectView _flameObjectView;
        
        private InGameBaseUIPresenter _inGameBaseUIPresenter;
        private FlamePresenter _flamePresenter;

        public override void InstallBindings()
        {
            var sceneController = ServiceLocator.Resolve<ISceneController>();
            _inGameBaseUIPresenter = new InGameBaseUIPresenter(_inGameBaseUI, sceneController);

            _flamePresenter = new FlamePresenter(_flameColorControllerView, _flameObjectView);
        }

        public override void Initialize()
        {
            _inGameBaseUIPresenter.Initialize();
            _flamePresenter.Initialize();
        }

        public override void Dispose()
        {
            _inGameBaseUIPresenter.Dispose();
            _flamePresenter.Dispose();
        }
    }
}