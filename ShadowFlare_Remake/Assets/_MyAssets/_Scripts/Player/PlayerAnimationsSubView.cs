using System;
using UnityEngine;

namespace ShadowFlareRemake.Player
{
    public class PlayerAnimationsSubView : MonoBehaviour
    {
        public event Action OnDo_StepForwardAnimationEvent;
        public event Action OnFinished_MeleeSingleAttack;
        public event Action OnFinished_MeleeTripleAttack;

        public void DoStepForward()
        {
            OnDo_StepForwardAnimationEvent?.Invoke();
        }

        public void FinishedMeleeSingleAttack()
        {
            OnFinished_MeleeSingleAttack?.Invoke();
        }

        public void FinishedMeleeTripleAttack()
        {
            OnFinished_MeleeTripleAttack?.Invoke();
        }
    }
}
