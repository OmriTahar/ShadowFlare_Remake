using ShadowFlareRemake.Enums;
using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public class EnemyModel : Model {

        public IEnemyUnit Unit { get; private set; }
        public IUnitHandler UnitHandler { get; private set; }

        public Color Color { get; private set; }
        public string Name { get; private set; }

        public AttackType CurrentAttackType { get; private set; }
        public AttackMethod CurrentAttackMethod { get; private set; }

        public bool IsAttacking { get; private set; } = false;
        public bool IsEnemyHighlighted { get; private set; } = false;


        public EnemyModel(IEnemyUnit unit, IUnitHandler unitHandler) {

            Unit = unit;
            UnitHandler = unitHandler;

            Color = unit.Color;
            Name = unit.Name;
        }

        public void SetEnemyUnitAndUnitHandler(IEnemyUnit unit, IUnitHandler unitHandler) {

            Unit = unit;
            UnitHandler = unitHandler;
            Changed();
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

