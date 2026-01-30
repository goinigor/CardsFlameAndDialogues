using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.Dialogues
{
    public class DialoguesPresenter : IDisposable
    {
        public event Action OnNextDialogueRequested;
        
        private readonly DialoguesView _view;

        public DialoguesPresenter(DialoguesView view)
        {
            _view = view;
        }
        
        public void Show()
        {
            _view.Show();
            _view.OnNextDialogueRequested += OnNextDialogue;
        }

        public void Hide()
        {
            _view.OnNextDialogueRequested -= OnNextDialogue;
            _view.Hide();
        }

        private void OnNextDialogue()
        {
            OnNextDialogueRequested?.Invoke();
        }
        
        public void SetNextDialogue(string userName, string text, UniTask<Sprite> avatarSprite, AvatarPosition side)
        {
            _view.SetDialogueText(text);
            _view.SetUserName(userName);
            _view.SetUserIconSide(side);
            _view.SetUserIcon(avatarSprite);
        }

        public void Dispose()
        {
            _view.OnNextDialogueRequested -= OnNextDialogue;
        }
    }
}