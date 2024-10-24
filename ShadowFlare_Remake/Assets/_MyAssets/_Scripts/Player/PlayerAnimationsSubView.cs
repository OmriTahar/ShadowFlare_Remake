using System;
using UnityEngine;

namespace ShadowFlareRemake.Player
{
    public class PlayerAnimationsSubView : MonoBehaviour
    {
        public event Action OnDo_StepForwardAnimationEvent;
        public event Action OnFinished_MeleeSingleAttack;
        public event Action OnFinished_MeleeTripleAttack;
        public event Action OnFinishedAttackAnimation;

        public void DoStepForward()                   // Called from an animation event (Melee Triple 1-3)
        {
            OnDo_StepForwardAnimationEvent?.Invoke();
        }

        public void FinishedMeleeSingleAttack()       // Called from an animation event (Melee Single)
        {
            OnFinished_MeleeSingleAttack?.Invoke();
        }

        public void FinishedMeleeTripleAttack()       // Called from an animation event (Melee Triple_3)
        {
            OnFinished_MeleeTripleAttack?.Invoke();
        }

        public void FinishedAttackAnimation()         // Called from an animation event 
        {
            OnFinishedAttackAnimation?.Invoke();
        }
    }
}
