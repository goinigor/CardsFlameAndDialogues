using System;

namespace CFD.Features.PhoenixFlame.Presenter
{
    public class FlamePresenter : IDisposable
    {
        private readonly FlameColorControllerView _controllerView;
        private readonly FlameObjectView _flameObjectView;

        public FlamePresenter(FlameColorControllerView controllerView, FlameObjectView flameObjectView)
        {
            _controllerView = controllerView;
            _flameObjectView = flameObjectView;
        }

        public void Initialize()
        {
            _controllerView.OnButtonClicked += OnButtonClick;
        }

        private void OnButtonClick(int animatorTrigger)
        {
            _flameObjectView.SetTrigger(animatorTrigger);
        }


        public void Dispose()
        {
            _controllerView.OnButtonClicked -= OnButtonClick;
        }
    }
}