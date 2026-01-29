using System.Threading.Tasks;
using CFD.Core;
using CFD.Features.Dialogues;
using UnityEngine;

namespace CFD.UI.InGame
{
    public class MagicWordsInstaller : AbstractMonoInstaller
    {
        [SerializeField] private InGameBaseUI _inGameBaseUI;
        [SerializeField] private string _url;
        [SerializeField] private DialoguesConfig _dialoguesConfig;
        [SerializeField] private DialoguesView _dialoguesView;
        
        private InGameBaseUIPresenter _inGameBaseUIPresenter;
        private DialoguesSystem _dialoguesSystem;
        private API _api;

        public override void InstallBindings()
        {
            var sceneController = ServiceLocator.Resolve<ISceneController>();
            _inGameBaseUIPresenter = new InGameBaseUIPresenter(_inGameBaseUI, sceneController);
            _api = ServiceLocator.Resolve<API>();

            _dialoguesSystem = new DialoguesSystem(_dialoguesConfig, _api, _dialoguesView, sceneController);
        }

        public override void Initialize()
        {
            _inGameBaseUIPresenter.Initialize();
            _dialoguesSystem.Initialize();
        }

        public override void Dispose()
        {
            _inGameBaseUIPresenter.Dispose();
            _dialoguesSystem.Dispose();
        }
    }
}