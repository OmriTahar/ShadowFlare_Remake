using ShadowFlareRemake.Combat;

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
            Model.SetAttackState(false, AttackRange.None);
            Agent.isStopped = true;

            if(!IsActive)
                return;

            if(DistanceFromPlayer <= Model.Stats.AttackDistance)
            {
                Model.SetEnemyState(EnemyState.Attacking);
            }
            else if(DistanceFromPlayer <= Model.Stats.ChaseDistance)
            {
                Model.SetEnemyState(EnemyState.Chasing);
            }
        }

        private void HandleChasingState()
        {
            Agent.isStopped = false;

            Agent.SetDestination(PlayerTransform.position);

            if((DistanceFromPlayer <= Model.Stats.AttackDistance) || (DistanceFromPlayer > Model.Stats.ChaseDistance))
            {
                Model.SetEnemyState(EnemyState.Idle);
            }
        }

        private void HandleAttackState()
        {
            Agent.isStopped = true;
            transform.LookAt(PlayerTransform);

            if(!Model.IsAttacking)
            {
                Model.SetAttackState(true, AttackRange.Close);
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

