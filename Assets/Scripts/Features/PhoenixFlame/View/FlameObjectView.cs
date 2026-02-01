using UnityEngine;

namespace CFD.Features.PhoenixFlame
{
    public class FlameObjectView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetTrigger(int trigger)
        {
            _animator.SetTrigger(trigger);
        }
    }
}