using System;
using CFD.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CFD.Features.PhoenixFlame
{
    public class FlameColorControllerView : View
    {
        private readonly int ANIMATOR_ORANGE = Animator.StringToHash("Orange");
        private readonly int ANIMATOR_GREEN = Animator.StringToHash("Green");
        private readonly int ANIMATOR_BLUE = Animator.StringToHash("Blue");
        private readonly int ANIMATOR_LOOP = Animator.StringToHash("Loop");
        private readonly int ANIMATOR_LOOP_SINGLE_ANIMATION = Animator.StringToHash("LoopSingleAnimation");

        public event Action<int> OnButtonClicked;
        
        [SerializeField] private Button _orangeButton;
        [SerializeField] private Button _greenButton;
        [SerializeField] private Button _blueButton;
        [SerializeField] private Button _animatorLoopButton;
        [SerializeField] private Button _animationLoopButton;

        private void Awake()
        {
            _orangeButton.onClick.AddListener(OnOrangeButtonClick);
            _greenButton.onClick.AddListener(OnGreenButtonClick);
            _blueButton.onClick.AddListener(OnBlueButtonClick);
            _animatorLoopButton.onClick.AddListener(OnAnimatorLoopButtonClick);
            _animationLoopButton.onClick.AddListener(OnAnimationLoopButtonClick);
        }

        protected override void VirtualOnDestroy()
        {
            base.VirtualOnDestroy();
            _orangeButton.onClick.RemoveListener(OnOrangeButtonClick);
            _greenButton.onClick.RemoveListener(OnGreenButtonClick);
            _blueButton.onClick.RemoveListener(OnBlueButtonClick);
            _animatorLoopButton.onClick.RemoveListener(OnAnimatorLoopButtonClick);
            _animationLoopButton.onClick.RemoveListener(OnAnimationLoopButtonClick);
        }

        private void OnOrangeButtonClick()
        {
            OnButtonClicked?.Invoke(ANIMATOR_ORANGE);
        }

        private void OnGreenButtonClick()
        {
            OnButtonClicked?.Invoke(ANIMATOR_GREEN);
        }

        private void OnBlueButtonClick()
        {
            OnButtonClicked?.Invoke(ANIMATOR_BLUE);
        }

        private void OnAnimatorLoopButtonClick()
        {
            OnButtonClicked?.Invoke(ANIMATOR_LOOP);
        }

        private void OnAnimationLoopButtonClick()
        {
            OnButtonClicked?.Invoke(ANIMATOR_LOOP_SINGLE_ANIMATION);
        }
    }
}