using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Enemies
{
    public class SkeletonController : EnemyController
    {
        protected override void Update()
        {
            base.Update();
            HandleCurrentEnemyState();
        }

        private void HandleCurrentEnemyState()
        {
            switch(Model.CurrentEnemyState)
            {
                case EnemyState.Idle:
                    HandleIdleState();
                    break;

                case EnemyState.Chasing:
                    HandleChasingState();
                    break;

                case EnemyState.Attacking:
                    break;

                case EnemyState.Dead:
                    HandleDeadState();
                    break;

                default:
                    break;
            }
        }


        private void HandleIdleState()
        {
            Agent.isStopped = true;

            if(DistanceFromPlayer > Model.Stats.AttackDistance)
            {
                Model.SetEnemyState(EnemyState.Chasing);
            }
        }


        private void HandleChasingState()
        {
            Agent.isStopped = false;
            Agent.SetDestination(PlayerTransform.position);

            if(DistanceFromPlayer <= Model.Stats.AttackDistance)
            {
                Model.SetEnemyState(EnemyState.Idle);
            }
        }

        private void HandleDeadState()
        {
            Agent.isStopped = true;
        }

        private void Attack()
        {
            Model.UpdateAttackState(true, AttackMethod.Close);
            IsAllowedToAttack = false;
        }
    }
}

