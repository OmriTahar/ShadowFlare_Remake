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
                    HandleAttackState();
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
            Model.SetAttackState(false, AttackMethod.None);
            Agent.isStopped = true;

            if(DistanceFromPlayer > Model.Stats.AttackDistance)
            {
                Model.SetEnemyState(EnemyState.Chasing);
            }
            else
            {
                Model.SetEnemyState(EnemyState.Attacking);
            }
        }


        private void HandleChasingState()
        {
            Agent.isStopped = false;
            Agent.SetDestination(PlayerTransform.position);

            if(DistanceFromPlayer <= Model.Stats.AttackDistance)
            {
                Model.SetEnemyState(EnemyState.Attacking);
            }
        }

        private void HandleAttackState()
        {
            Agent.isStopped = true;
            transform.LookAt(PlayerTransform);

            if(!Model.IsAttacking)
            {
                Model.SetAttackState(true, AttackMethod.Close);
            }
        }

        protected override void HandleAttackAnimationEnded()
        {
            Model.SetEnemyState(EnemyState.Idle);
        }

        private void HandleDeadState()
        {
            Agent.isStopped = true;
        }
    }
}
