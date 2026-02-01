using System;
using UnityEngine;

namespace CFD.Misc
{
    /// <summary>
    /// This class is used for catching animation triggers to pass it further into the code.
    /// Must be attached onto the object with animator/animation on it.
    ///
    /// Useful case if the handler of the event is not the same object with the animator, and you have no possibility
    /// to call a callback method on the handler
    /// </summary>
    public class AnimationTriggerHandler : MonoBehaviour
    {
        public event Action OnTriggered;
        public event Action OnTriggered1;

        public void Trigger()
        {
            OnTriggered?.Invoke();
        }

        public void Trigger1()
        {
            OnTriggered1?.Invoke();
        }
    }
}