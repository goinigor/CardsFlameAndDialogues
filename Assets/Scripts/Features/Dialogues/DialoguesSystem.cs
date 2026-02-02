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
        private CancellationTokenSource _currentDialogueCTS;

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
            //if data was loaded successfully, show dialogues, else show main menu
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
            DisposeCurrentDialogueCTS();
            
            _currentDialogueCTS = new CancellationTokenSource();

            // Find next existing dialogue
            DialogueLine currentDialogue = null;
            while (currentDialogue == null)
            {
                _currentDialogueIndex++;

                if (_currentDialogueIndex >= _dialoguesData.dialogue.Count)
                {
                    _dialoguesPresenter.Hide();
                    _dialoguesPresenter.OnNextDialogueRequested -= ShowNextDialogue;
                    return;
                }
                
                currentDialogue = _dialoguesData.dialogue[_currentDialogueIndex];
            }

            var avatarData = _dialoguesData.avatars?.FirstOrDefault(a => a.name == currentDialogue.name);
            var avatarSprite = avatarData != null ? GetAvatar(avatarData) : UniTask.FromResult<Sprite>(_config.FallbackUserIcon);
            var side = avatarData?.GetPosition() ?? AvatarPosition.left;
            
            _dialoguesPresenter.SetNextDialogue(currentDialogue.name, currentDialogue.GetTextWithEmotions(), avatarSprite, side);
        }

        private async UniTask<Sprite> GetAvatar(AvatarData avatarData)
        {
            if (avatarData == null)
                return null;
            
            using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, _currentDialogueCTS.Token);
            var token = linkedToken.Token;
            var sprite = await _dialogueAvatarsCache.GetAvatar(avatarData, token);
            
            if (token.IsCancellationRequested)
                return null;

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

        private void DisposeCurrentDialogueCTS()
        {
            _currentDialogueCTS?.Cancel();
            _currentDialogueCTS?.Dispose();
            _currentDialogueCTS = null;
        }
        
        public void Dispose()
        {
            DisposeCTS();
            DisposeCurrentDialogueCTS();
            _dialogueAvatarsCache.Dispose();
            _dialoguesPresenter.OnNextDialogueRequested -= ShowNextDialogue;
        }
    }
}