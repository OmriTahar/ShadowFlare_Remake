
namespace ShadowFlareRemake.Player {
    public class PlayerModel : Model {

        public IUnit Unit { get; private set; }

        public enum AttackType { None, Single, ThreeStrikes }
        public AttackType CurrentAttackType { get; private set; }

        public int MovementSpeed { get; private set; }
        public bool IsAttacking { get; private set; } = false;
        public bool CanTakeDamage { get; private set; } = true;

        public PlayerModel(IUnit unit, bool isAttacking = false, AttackType attackType = AttackType.None) {

            Unit = unit;
            MovementSpeed = unit.Stats.MovementSpeed;
            IsAttacking = isAttacking;
            CurrentAttackType = attackType;
        }

        public void SetUnit(IUnit unit) {

            Unit = unit;
            Changed();
        }

        public void SetAttackState(bool isAttacking, AttackType attackType = AttackType.None) {

            IsAttacking = isAttacking;
            CurrentAttackType = attackType;
            Changed();
        }
    }
}

