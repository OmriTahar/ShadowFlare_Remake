using ShadowFlareRemake.Player;
using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.PlayerRestrictedData
{
    public class PlayerModel : BasePlayerModel
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

        public void SetActiveSkill(ISkillData activeSkill)
        {
            ActiveSkill = activeSkill;
            Changed();
        }

        public void SetAttackState(bool isAttacking, bool isUsingSkill)
        {
            IsAttacking = isAttacking;
            IsUsingSkill = isUsingSkill;

            if(isAttacking)
            {
                IsMoving = false;
            }

            Changed();
        }


        #endregion

        #region Meat & Potatos - Overrides

        public override void SetIsAttackingToFalse()
        {
            IsAttacking = false;
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

        public override float GetAttackSpeedForPhysicalAttackAnimations()
        {
            return Stats.AttackSpeed / _animationsSpeedAdjuster;
        }

        public override float GetAttackSpeedForMagicalAttackAnimations()
        {
            return Stats.MagicalAttackSpeed / _animationsSpeedAdjuster;
        }

        #endregion
    }
}

