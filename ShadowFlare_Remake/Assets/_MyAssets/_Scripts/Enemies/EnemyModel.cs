using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Enemies
{
    public class EnemyModel : Model
    {
        public IUnit Unit { get; private set; }
        public IEnemyUnitStats Stats { get; private set; }
        public EnemyState CurrentEnemyState { get; private set; }

        public string Name { get; private set; }
        public Color Color { get; private set; }

        public AttackDamageType CurrentAttackType { get; private set; }
        public AttackMethodType CurrentAttackMethod { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool IsReceivedCritialHit { get; private set; }

        public EnemyModel(IUnit unit)
        {
            Unit = unit;
            Stats = unit.Stats as IEnemyUnitStats;
            Name = Stats.Name;
            Color = new Color(Stats.Color.r, Stats.Color.g, Stats.Color.b, 1);
        }

        public void SetEnemyUnitAfterHit(IUnit unit, bool isReceivedCritialHit = false)
        {
            Unit = unit;
            Stats = Unit.Stats as IEnemyUnitStats;
            IsReceivedCritialHit = isReceivedCritialHit;
            Changed();
        }

        public void SetAttackState(bool isAttacking, AttackMethodType attackMethod)
        {
            IsAttacking = isAttacking;
            CurrentAttackMethod = attackMethod;
            Changed();
        }

        public void SetEnemyState(EnemyState enemyState)
        {
            if(CurrentEnemyState == enemyState)
                return;

            CurrentEnemyState = enemyState;
            Changed();
        }

        private void PrintEnemyStateTransition(EnemyState enemyState)
        {
            string oldState = CurrentEnemyState.ToString().ToUpper();
            string newState = enemyState.ToString().ToUpper();
            Debug.Log($"{Name} is switching from {oldState} to {newState}.");
        }
    }
}

