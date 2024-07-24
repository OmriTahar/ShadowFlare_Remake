using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.Player
{
    public class PlayerModel : Model
    {
        public IUnit Unit { get; private set; }
        public IPlayerUnitStats Stats { get; private set; }

        public PlayerAttack CurrentPlayerAttack { get; private set; }
        public bool IsAttacking { get; private set; } = false;
        public bool IsLastHitWasCritialHit { get; private set; }

        public bool IsMoving { get; private set; }
        public bool CanTakeDamage { get; private set; } = true;

        private const int _movementSpeedLogicAdjuster = 20;
        private const float _animationsSpeedAdjuster = 100;

        public PlayerModel(IUnit unit)
        {
            Unit = unit;
            Stats = unit.Stats as IPlayerUnitStats;
        }

        public void SetAttackState(bool isAttacking, PlayerAttack playerAttack)
        {
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

        public void SetIsLastHitWasCritialHit(bool isLastHitWasCritialHit)
        {
            IsLastHitWasCritialHit = isLastHitWasCritialHit;
            Changed();
        }

        public int GetMovementSpeedForMoveLogic()
        {
            return Stats.MovementSpeed / _movementSpeedLogicAdjuster;
        }

        public float GetMovementSpeedForMoveAnimation()
        {
            return Stats.MovementSpeed / _animationsSpeedAdjuster;
        }

        public float GetAttackSpeedForAttackAnimations()
        {
            return Stats.AttackSpeed / _animationsSpeedAdjuster;
        }
    }
}

