using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.Player
{
    public abstract class BasePlayerModel : Model
    {
        public IUnit Unit { get; protected set; }
        public IPlayerUnitStats Stats { get; protected set; }

        public ISkillData ActiveSkill { get; protected set; }
        public bool IsAttacking { get; protected set; }
        public bool IsUsingSkill { get; protected set; }
        public bool IsLastHitWasCritialHit { get; protected set; }
        public bool IsMoving { get; protected set; }
        public bool IsInteracting { get; protected set; }

        public abstract void SetIsAttackingToFalse();
        public abstract void SetIsMoving(bool isMoving);
        public abstract void SetIsInteracting(bool isInteracting, bool invokeChanged = true);
        public abstract int GetMovementSpeedForMoveLogic();
        public abstract float GetMovementSpeedForMoveAnimation();
        public abstract float GetAttackSpeedForPhysicalAttackAnimations();
        public abstract float GetAttackSpeedForMagicalAttackAnimations();
    }
}
