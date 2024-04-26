using ShadowFlareRemake.Enums;
using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public class GoblinController : EnemyController {

        protected override void Update() {

            base.Update();

            if(!IsActive || !Agent.isActiveAndEnabled) {
                return;
            }

            if(DistanceFromPlayer < AttackDistance) {

                ChasePlayer = false;
                Agent.isStopped = true;

                if(IsAllowedToAttack) {

                    Attack();
                }

            } else {

                ChasePlayer = true;
                Agent.isStopped = false;
            }
        }

        protected override void Attack() {

            Model.UpdateAttackState(true, AttackMethod.Close);
            IsAllowedToAttack = false;
        }
    }
}

