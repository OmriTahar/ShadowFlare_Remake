using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Enemies
{
    public class SkeletonController : EnemyController
    {
        protected override void Update()
        {
            base.Update();

            //if(!IsActive || !Agent.isActiveAndEnabled)
            //{
            //    return;
            //}

            if(ChasePlayer)
            {
                Agent.SetDestination(PlayerTransform.position);
                Model.SetEnemyState(EnemyState.Chasing);
            }

            if(DistanceFromPlayer < AttackDistance)
            {
                ChasePlayer = false;
                Agent.isStopped = true;
                Model.SetEnemyState(EnemyState.Idle);

                //if(IsAllowedToAttack)
                //{
                //    Attack();
                //}

            }
            else if(ChasePlayer)
            {
                ChasePlayer = true;
                Agent.isStopped = false;
                Model.SetEnemyState(EnemyState.Chasing);
            }
        }

        protected override void Attack()
        {
            Model.UpdateAttackState(true, AttackMethod.Close);
            IsAllowedToAttack = false;
        }
    }
}

