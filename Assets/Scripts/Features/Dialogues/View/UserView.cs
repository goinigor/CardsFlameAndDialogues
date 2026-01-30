using CFD.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CFD.Features.Dialogues
{
    public class UserView : View
    {
        [SerializeField] private TMP_Text _userName;
        [SerializeField] private Image _userIcon;
        [SerializeField] private GameObject _imageContainer;
        [SerializeField] private GameObject _loadingObject;
        
        public void SetUserName(string name)
        {
            _userName.text = name;
        }
        
        public void SetUserIcon(Sprite sprite)
        {
            _userIcon.sprite = sprite;
            
            _loadingObject.SetActive(false);
            _imageContainer.SetActive(true);
        }

        public void SetUserIconLoading()
        {
            _loadingObject.SetActive(true);
            _imageContainer.SetActive(false);
        }
    }
}