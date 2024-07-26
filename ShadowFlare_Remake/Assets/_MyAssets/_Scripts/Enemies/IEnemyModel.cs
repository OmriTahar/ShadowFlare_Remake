using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Enemies
{
    public abstract class IEnemyModel : Model
    {
        public IUnit Unit { get; protected set; }
        public IEnemyUnitStats Stats { get; protected set; }
        public EnemyState CurrentEnemyState { get; protected set; }

        public string Name { get; protected set; }
        public Color Color { get; protected set; }

        public AttackRange CurrentAttackRange { get; protected set; }
        public bool IsAttacking { get; protected set; }
        public bool IsReceivedCritialHit { get; protected set; }

        public abstract void SetAttackState(bool isAttacking, AttackRange attackRange);
        public abstract void SetEnemyState(EnemyState enemyState);
    }
}
