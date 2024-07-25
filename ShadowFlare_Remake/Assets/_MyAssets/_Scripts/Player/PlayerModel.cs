using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.Player
{
    public class PlayerModel : Model
    {
        public IUnit Unit { get; private set; }
        public IPlayerUnitStats Stats { get; private set; }

        public SkillType ActiveSkill { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool IsSingleMeleeAttack { get; private set; }
        public bool IsLastHitWasCritialHit { get; private set; }
        public bool IsMoving { get; private set; }

        private const int _movementSpeedLogicAdjuster = 20;
        private const float _animationsSpeedAdjuster = 100;

        #region Initialization

        public PlayerModel(IUnit unit)
        {
            Unit = unit;
            Stats = unit.Stats as IPlayerUnitStats;
        }

        #endregion

        #region Meat & Potatos

        public void SetAttackState(bool isAttacking, bool isSingleMeleeAttack)
        {
            IsAttacking = isAttacking;
            IsSingleMeleeAttack = isSingleMeleeAttack;

            if(isAttacking)
            {
                IsMoving = false;
            }

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

        public void SetActiveSkill(SkillType activeSkill)
        {
            ActiveSkill = activeSkill;
            Changed();
        }

        #endregion

        #region Getters

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

        #endregion
    }
}

