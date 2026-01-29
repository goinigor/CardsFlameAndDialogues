using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.Dialogues
{
    public class DialoguesPresenter : IDisposable
    {
        private readonly DialoguesView _view;

        public DialoguesPresenter(DialoguesView view)
        {
            _view = view;
        }
        
        public void Show()
        {
            _view.Show();
        }

        public void Hide()
        {
            _view.Hide();
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
            
        }
    }
}