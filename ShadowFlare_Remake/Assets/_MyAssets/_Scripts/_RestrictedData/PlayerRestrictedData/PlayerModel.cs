using ShadowFlareRemake.Player;
using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.PlayerRestrictedData
{
    public class PlayerModel : IPlayerModel
    {
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

        #region Meat & Potatos - Overrides

        public override void SetAttackState(bool isAttacking, bool isSingleMeleeAttack)
        {
            IsAttacking = isAttacking;
            IsSingleMeleeAttack = isSingleMeleeAttack;

            if(isAttacking)
            {
                IsMoving = false;
            }

            Changed();
        }

        public override void SetIsMoving(bool isMoving)
        {
            IsMoving = isMoving;
            Changed();
        }

        public override int GetMovementSpeedForMoveLogic()
        {
            return Stats.MovementSpeed / _movementSpeedLogicAdjuster;
        }

        public override float GetMovementSpeedForMoveAnimation()
        {
            return Stats.MovementSpeed / _animationsSpeedAdjuster;
        }

        public override float GetAttackSpeedForAttackAnimations()
        {
            return Stats.AttackSpeed / _animationsSpeedAdjuster;
        }

        #endregion
    }
}

