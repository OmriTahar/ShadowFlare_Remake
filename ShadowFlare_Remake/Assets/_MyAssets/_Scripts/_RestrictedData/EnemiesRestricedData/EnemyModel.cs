using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.EnemiesRestrictedData
{
    public class EnemyModel : IEnemyModel
    {
        public EnemyModel(IUnit unit)
        {
            Unit = unit;
            Stats = unit.Stats as IEnemyUnitStats;
            Name = Stats.Name;
            Color = new Color(Stats.Color.r, Stats.Color.g, Stats.Color.b, 1);
        }

        #region Meat & Potatos

        public void SetIsReceivedCritialHit(bool isReceivedCritialHit)
        {
            IsReceivedCritialHit = isReceivedCritialHit;
            Changed();
        }

        #endregion

        #region Meat & Potatos - Overrides

        public override void SetAttackState(bool isAttacking, AttackRange attackRange)
        {
            IsAttacking = isAttacking;
            CurrentAttackRange = attackRange;
            Changed();
        }

        public override void SetEnemyState(EnemyState enemyState)
        {
            if(CurrentEnemyState == enemyState)
                return;

            CurrentEnemyState = enemyState;
            Changed();
        }

        #endregion

        #region Test

        private void PrintEnemyStateTransition(EnemyState enemyState)
        {
            string oldState = CurrentEnemyState.ToString().ToUpper();
            string newState = enemyState.ToString().ToUpper();
            Debug.Log($"{Name} is switching from {oldState} to {newState}.");
        }

        #endregion
    }
}

