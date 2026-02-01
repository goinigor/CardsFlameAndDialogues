using System;
using CFD.Core;
using CFD.Core.UI;
using CFD.Misc;
using UnityEngine;

namespace CFD.UI
{
    public class LoadingScreen : View, IService
    {
        private readonly int ANIMATOR_DISAPPEAR = Animator.StringToHash("Disappear");

        public event Action OnShowAnimationEnded;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationTriggerHandler _animationTriggerHandler;

        private bool _isActive;

        public override void Show()
        {
            base.Show();
            _animationTriggerHandler.OnTriggered += OnShowAnimationEnd;
            _isActive = true;
        }

        private void OnShowAnimationEnd()
        {
            _animationTriggerHandler.OnTriggered -= OnShowAnimationEnd;
            OnShowAnimationEnded?.Invoke();
        }

        public override void Hide()
        {
            if (!_isActive)
                return;
            
            _animator.SetTrigger(ANIMATOR_DISAPPEAR);
            _animationTriggerHandler.OnTriggered1 += OnHideAnimationEnd;   
        }

        private void OnHideAnimationEnd()
        {
            _animationTriggerHandler.OnTriggered1 -= OnHideAnimationEnd;
            base.Hide();
        }

        protected override void VirtualOnDestroy()
        {
            base.VirtualOnDestroy();
            _animationTriggerHandler.OnTriggered -= OnShowAnimationEnd;
            _animationTriggerHandler.OnTriggered1 -= OnHideAnimationEnd;
            
            _isActive = false;
        }
    }
}