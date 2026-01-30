using System;
using System.Linq;
using System.Threading;
using CFD.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.Dialogues
{
    public class DialoguesSystem : IDisposable
    {
        private readonly DialoguesConfig _config;
        private readonly API _api;
        private readonly ISceneController _sceneController;
        private readonly DialoguesPresenter _dialoguesPresenter;
        private readonly DialogueAvatarsCache _dialogueAvatarsCache;

        private DialogueData _dialoguesData;
        private CancellationTokenSource _cancellationTokenSource;
        private int _currentDialogueIndex = -1;

        public DialoguesSystem(
            DialoguesConfig config,
            API api,
            DialoguesView dialoguesView,
            ISceneController sceneController
        )
        {
            _config = config;
            _api = api;
            _sceneController = sceneController;

            _dialoguesPresenter = new DialoguesPresenter(dialoguesView);
            _dialogueAvatarsCache = new DialogueAvatarsCache(_api);
        }

        public void Initialize()
        {
            DisposeCTS();
            
            _cancellationTokenSource = new CancellationTokenSource();
            _api.Get<DialogueData>(_config.DataUrl, DataLoadedSuccessfully, DataLoadFailed, _cancellationTokenSource.Token);
        }

        private void DataLoadedSuccessfully(DialogueData data)
        {
            _dialoguesData = data;
            
            _dialoguesPresenter.Show();
            _dialoguesPresenter.OnNextDialogueRequested += ShowNextDialogue;
            ShowNextDialogue();
        }

        private void ShowNextDialogue()
        {
            _currentDialogueIndex++;

            if (_currentDialogueIndex >= _dialoguesData.dialogue.Count)
            {
                _dialoguesPresenter.Hide();
                _dialoguesPresenter.OnNextDialogueRequested -= ShowNextDialogue;
                return;
            }
            
            var currentDialogue = _dialoguesData.dialogue[_currentDialogueIndex];
            var avatarData = _dialoguesData.avatars.FirstOrDefault(a => a.name == currentDialogue.name);
            var avatarSprite = GetAvatar(avatarData);
            _dialoguesPresenter.SetNextDialogue(currentDialogue.name, currentDialogue.GetTextWithEmotions(), avatarSprite, avatarData.GetPosition());
        }

        private async UniTask<Sprite> GetAvatar(AvatarData avatarData)
        {
            var sprite = await _dialogueAvatarsCache.GetAvatar(avatarData, _cancellationTokenSource.Token);

            if (sprite == null)
                sprite = _config.FallbackUserIcon;
            
            return sprite;
        }

        private void DataLoadFailed(string errorMsg)
        {
            _sceneController.LoadMainMenu();
        }

        private void DisposeCTS()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void Dispose()
        {
            DisposeCTS();
            _dialoguesPresenter.OnNextDialogueRequested -= ShowNextDialogue;
        }
    }
}