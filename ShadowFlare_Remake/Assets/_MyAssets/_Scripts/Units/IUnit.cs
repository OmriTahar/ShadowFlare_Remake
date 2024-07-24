
using ShadowFlareRemake.Skills;

namespace ShadowFlareRemake.Units
{
    public interface IUnit
    {
        IUnitStats Stats { get; }
        IUnitSkills Skills { get; }

        public int CurrentHP { get; }
        public int CurrentMP { get; }

        public void TakeDamage(int damage);
    }
}
