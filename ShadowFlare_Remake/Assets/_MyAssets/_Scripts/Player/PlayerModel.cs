using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.Player
{
    public class PlayerModel : Model
    {
        public IUnit Unit { get; private set; }
        public IPlayerUnitStats Stats { get; private set; }

        public SkillType ActiveSkill { get; private set; }
        public bool IsAttacking { get; private set; } = false;
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

        public void SetAttackState(bool isAttacking, SkillType activeSkill)
        {
            IsAttacking = isAttacking;
            ActiveSkill = activeSkill;

            if(isAttacking)
            {
                IsMoving = false;
            }

            Changed();

            if(ActiveSkill == SkillType.MeleeSingle) // Todo: Change this when granting threeStrike upon level 5
            {
                ActiveSkill = SkillType.MeleeTriple;
            }
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

