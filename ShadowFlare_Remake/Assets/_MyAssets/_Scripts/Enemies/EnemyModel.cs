using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Enemies {
    public class EnemyModel : Model {

        public string Name { get; private set; }
        public IUnit Unit { get; private set; }

        public AttackType CurrentAttackType { get; private set; }
        public AttackMethod CurrentAttackMethod { get; private set; }

        public bool IsAttacking { get; private set; } = false;
        public bool IsEnemyHighlighted { get; private set; } = false;

        public EnemyModel(IUnit unit) {

            Unit = unit;
            Name = unit.Stats.Name;
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

