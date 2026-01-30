using System;
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
        private InputAction _clickAction;

        private void Start()
        {
            _submitAction = InputSystem.actions.FindAction("Submit");
            _clickAction = InputSystem.actions.FindAction("Click");
        }

        private void Update()
        {
            if (_submitAction.WasPressedThisFrame() || _clickAction.WasPressedThisFrame())
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

        public async void SetUserIcon(UniTask<Sprite> sprite)
        {
            _leftView.SetUserIconLoading();
            _rightView.SetUserIconLoading();
            
            var downloadedSprite = await sprite;
            
            _leftView.SetUserIcon(downloadedSprite);
            _rightView.SetUserIcon(downloadedSprite);
        }
    }
}