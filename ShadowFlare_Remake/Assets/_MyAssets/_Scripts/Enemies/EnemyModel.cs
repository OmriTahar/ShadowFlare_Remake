using ShadowFlareRemake.Enums;
using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public class EnemyModel : Model {

        public string Name { get; private set; }
        public IUnit Unit { get; private set; }

        public AttackType CurrentAttackType { get; private set; }
        public AttackMethod CurrentAttackMethod { get; private set; }

        public bool IsAttacking { get; private set; } = false;
        public bool IsEnemyHighlighted { get; private set; } = false;

        public Color Color { get; private set; }

        public EnemyModel(IUnit unit) {

            Unit = unit;
            Name = unit.Stats.Name;

            var enemyStats = unit.Stats as EnemyStats;
            Color = enemyStats.Color;
        }

        public void UpdateIsEnemyHighlighted(bool isEnemyHighlighted) {

            if(IsEnemyHighlighted == isEnemyHighlighted) {
                return;
            }

            IsEnemyHighlighted = isEnemyHighlighted;
            Changed();
        }

        public void UpdateAttackState(bool isAttacking, AttackMethod attackMethod) {

            IsAttacking = isAttacking;
            CurrentAttackMethod = attackMethod;
            Changed();
        }

        public void InvokeChanged() {
            Changed();
        }
    }
}

