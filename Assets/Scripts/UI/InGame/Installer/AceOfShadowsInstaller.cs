using CFD.Core;
using CFD.Features.CardsShuffle;
using UnityEngine;

namespace CFD.UI.InGame
{
    public class AceOfShadowsInstaller : AbstractMonoInstaller
    {
        [SerializeField] private InGameBaseUI _inGameBaseUI;
        [SerializeField] private CardsAnimationBehaviourCurves _cardsAnimationBehaviour;
        [SerializeField] private CardsShuffleConfig _cardsShuffleConfig;
        
        private InGameBaseUIPresenter _inGameBaseUIPresenter;
        private CardsShuffleSystem _cardsShuffleSystem;

        public override void InstallBindings()
        {
            var sceneController = ServiceLocator.Resolve<ISceneController>();
            _inGameBaseUIPresenter = new InGameBaseUIPresenter(_inGameBaseUI, sceneController);
            
            _cardsShuffleSystem = new CardsShuffleSystem(_cardsShuffleConfig, _cardsAnimationBehaviour);
        }

        public override void Initialize()
        {
            _inGameBaseUIPresenter.Initialize();
            _cardsShuffleSystem.Initialize();
        }

        public override void Dispose()
        {
            _inGameBaseUIPresenter.Dispose();
            _cardsShuffleSystem.Dispose();
        }
    }
}