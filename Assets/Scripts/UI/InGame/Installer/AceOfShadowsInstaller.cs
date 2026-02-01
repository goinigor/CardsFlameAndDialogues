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
        [SerializeField] private DeckCountView _startDeckCounter;
        [SerializeField] private DeckCountView _endDeckCounter;
        [SerializeField] private GameObject _endingText;
        
        private InGameBaseUIPresenter _inGameBaseUIPresenter;
        private CardsShuffleSystem _cardsShuffleSystem;
        private CardPool _cardsPool;

        public override void InstallBindings()
        {
            var sceneController = ServiceLocator.Resolve<ISceneController>();
            _cardsPool = new CardPool(_cardsShuffleConfig.CardMaterials, _cardsShuffleConfig.FallbackMaterial, _cardsShuffleConfig.CardPrefab);
            
            _inGameBaseUIPresenter = new InGameBaseUIPresenter(_inGameBaseUI, sceneController);

            _cardsShuffleSystem = new CardsShuffleSystem(
                _cardsShuffleConfig,
                _cardsAnimationBehaviour,
                _startDeckCounter,
                _endDeckCounter,
                _endingText,
                _cardsPool
            );
        }

        public override void Initialize()
        {
            _cardsPool.Initialize();
            _inGameBaseUIPresenter.Initialize();
            _cardsShuffleSystem.Initialize();
        }

        public override void Dispose()
        {
            _cardsPool.Clear();
            _inGameBaseUIPresenter.Dispose();
            _cardsShuffleSystem.Dispose();
        }
    }
}