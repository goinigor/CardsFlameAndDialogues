using System;
using System.Threading;
using CFD.Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CFD.Features.Dialogues
{
    public class DialoguesView : View, IPointerClickHandler
    {
        public event Action OnNextDialogueRequested;
        
        [SerializeField] private UserView _leftView;
        [SerializeField] private UserView _rightView;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private Sprite _loadingSprite;

        private InputAction _submitAction;
        private CancellationTokenSource _iconLoadCTS;

        private void Start()
        {
            _submitAction = InputSystem.actions.FindAction("Submit");
        }

        private void Update()
        {
            if (_submitAction.WasPressedThisFrame())
            {
                OnNextDialogue();
            }
        }

        public override void Show()
        {
            base.Show();
            InputSystem.actions.Enable();
        }

        public override void Hide()
        {
            base.Hide();
            InputSystem.actions.Disable();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnNextDialogue();
        }

        private void OnNextDialogue()
        {
            OnNextDialogueRequested?.Invoke();
        }

        public void SetDialogueText(string text)
        {
            _dialogueText.text = text;
        }

        public void SetUserName(string name)
        {
            _leftView.SetUserName(name);
            _rightView.SetUserName(name);
        }

        public void SetUserIconSide(AvatarPosition side)
        {
            switch (side)
            {
                case AvatarPosition.left:
                    _leftView.Show();
                    _rightView.Hide();
                    break;
                case AvatarPosition.right:
                    _leftView.Hide();
                    _rightView.Show();
                    break;
            }
        }

        public async void SetUserIcon(UniTask<Sprite> sprite, CancellationToken cancellationToken = default)
        {
            DisposeCTS();
            
            _iconLoadCTS = new CancellationTokenSource();
    
            _leftView.SetUserIconLoading();
            _rightView.SetUserIconLoading();

            var token = _iconLoadCTS.Token;
            var downloadedSprite = await sprite.AttachExternalCancellation(token).SuppressCancellationThrow();

            if (!token.IsCancellationRequested)
            {
                _leftView.SetUserIcon(downloadedSprite.Result);
                _rightView.SetUserIcon(downloadedSprite.Result);
            }
        }

        private void DisposeCTS()
        {
            _iconLoadCTS?.Cancel();
            _iconLoadCTS?.Dispose();
            _iconLoadCTS = null;
        }
    }
}
