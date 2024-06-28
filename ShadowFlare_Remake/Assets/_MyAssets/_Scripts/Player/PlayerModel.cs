
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Player {
    public class PlayerModel : Model {

        public IUnit Unit { get; private set; }
        public IPlayerUnitStats Stats { get; private set; }

        public PlayerAttack CurrentPlayerAttack { get; private set; }
        public bool IsAttacking { get; private set; } = false;
        public bool IsReceivedCritialHit { get; private set; }

        public bool IsMoving { get; private set; }
        public bool CanTakeDamage { get; private set; } = true;

        public PlayerModel(IUnit unit) {

            Unit = unit;
            Stats = unit.Stats as IPlayerUnitStats;
        }

        public void SetPlayerUnitAfterHeal(IUnit unit)
        {
            Unit = unit;
            Changed();
        }

        public void SetPlayerUnitAfterHit(IUnit unit, bool isReceivedCritialHit = false) {

            Unit = unit;
            Stats = unit.Stats as IPlayerUnitStats;
            IsReceivedCritialHit = isReceivedCritialHit;
            Changed();
        }

        public void SetAttackState(bool isAttacking, PlayerAttack playerAttack) {

            IsAttacking = isAttacking;
            CurrentPlayerAttack = playerAttack;

            if(playerAttack != PlayerAttack.None)
                IsMoving = false;
           
            Changed();
        }

        public void SetIsMoving(bool isMoving)
        {
            IsMoving = isMoving;
            Changed();
        }
    }
}

