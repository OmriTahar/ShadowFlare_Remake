
namespace ShadowFlareRemake.GameManager.Units
{
    public class Unit : IUnit
    {
        public IUnitStats Stats { get; private set; }
        public int CurrentHP { get; private set; }
        public int CurrentMP { get; private set; }

        public Unit(IUnitStats stats)
        {
            Stats = stats;
            CurrentHP = stats.MaxHP;
            CurrentMP = stats.MaxMP;
        }

        public void FullHeal()
        {
            CurrentHP = Stats.MaxHP;
            CurrentMP = Stats.MaxMP;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
        }
    }
}