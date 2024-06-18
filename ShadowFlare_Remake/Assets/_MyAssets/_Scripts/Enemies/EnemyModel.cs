using ShadowFlareRemake.Enums;
using UnityEngine;

namespace ShadowFlareRemake.Enemies
{
    public class EnemyModel : Model
    {
        public IUnit Unit { get; private set; }
        public IEnemyUnitStats Stats { get; private set; }

        public string Name { get; private set; }
        public Color Color { get; private set; }

        public AttackType CurrentAttackType { get; private set; }
        public AttackMethod CurrentAttackMethod { get; private set; }

        public EnemyState CurrentEnemyState { get; private set; }

        public bool IsAttacking { get; private set; } = false;

        public EnemyModel(IUnit unit)
        {
            Unit = unit;
            Stats = unit.Stats as IEnemyUnitStats;

            Name = Stats.Name;
            Color = new Color(Stats.Color.r, Stats.Color.g, Stats.Color.b, 1);
        }

        public void SetEnemyUnitAndUnitHandler(IUnit unit)
        {
            Unit = unit;
            Stats = Unit.Stats as IEnemyUnitStats;
            Changed();
        }

        public void UpdateAttackState(bool isAttacking, AttackMethod attackMethod)
        {
            IsAttacking = isAttacking;
            CurrentAttackMethod = attackMethod;
            Changed();
        }

        public void SetEnemyState(EnemyState enemyState)
        {
            CurrentEnemyState = enemyState; 
            Changed();
        }

        public void InvokeChanged()
        {
            Changed();
        }
    }
}

