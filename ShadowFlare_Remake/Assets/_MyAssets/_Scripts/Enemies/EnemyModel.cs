using ShadowFlareRemake.Enums;
using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public class EnemyModel : Model {

        public IUnit Unit { get; private set; }
        public IEnemyUnitStats Stats { get; private set; }

        public Color Color { get; private set; }
        public string Name { get; private set; }

        public AttackType CurrentAttackType { get; private set; }
        public AttackMethod CurrentAttackMethod { get; private set; }

        public bool IsAttacking { get; private set; } = false;
        public bool IsEnemyHighlighted { get; private set; } = false;


        public EnemyModel(IUnit unit) {

            Unit = unit;
            Stats = unit.Stats as IEnemyUnitStats;

            Color = Stats.Color;
            Name = Stats.Name;
        }

        public void SetEnemyUnitAndUnitHandler(IUnit unit) {

            Unit = unit;
            Stats = Unit.Stats as IEnemyUnitStats;
            Changed();
        }

        public void SetIsEnemyHighlighted(bool isEnemyHighlighted) {

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

