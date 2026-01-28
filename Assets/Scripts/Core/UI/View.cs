using System;
using UnityEngine;

namespace CFD.Core.UI
{
    public class View : MonoBehaviour
    {
        public event Action OnShow;
        public event Action OnHide;
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }

        private void OnDestroy()
        {
            VirtualOnDestroy();
        }

        protected virtual void VirtualOnDestroy()
        {
            OnShow = null;
            OnHide = null;
        }
    }
}