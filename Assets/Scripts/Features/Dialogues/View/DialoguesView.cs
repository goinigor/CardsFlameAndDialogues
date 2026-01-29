using CFD.Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CFD.Features.Dialogues
{
    public class DialoguesView : View
    {
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private TMP_Text _userNameLeft;
        [SerializeField] private TMP_Text _userNameRight;
        [SerializeField] private Image _userIconLeft;
        [SerializeField] private Image _userIconRight;
        [SerializeField] private Sprite _loadingSprite;

        public void SetDialogueText(string text)
        {
            _dialogueText.text = text;
        }

        public void SetUserName(string name)
        {
            _userNameLeft.text = name;
            _userNameRight.text = name;
        }

        public void SetUserIconSide(AvatarPosition side)
        {
            _userNameLeft.gameObject.SetActive(side == AvatarPosition.left);
            _userNameRight.gameObject.SetActive(side == AvatarPosition.right);
            
            _userIconLeft.gameObject.SetActive(side == AvatarPosition.left);
            _userIconRight.gameObject.SetActive(side == AvatarPosition.right);
        }
        
        public async void SetUserIcon(UniTask<Sprite> sprite)
        {
            //TODO add loading wheel instead of sprite
            _userIconLeft.sprite = _loadingSprite;
            _userIconRight.sprite = _loadingSprite;
            
            var downloadedSprite = await sprite;
            _userIconLeft.sprite = downloadedSprite;
            _userIconRight.sprite = downloadedSprite;
        }
    }
}