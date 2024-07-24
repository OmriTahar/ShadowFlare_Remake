using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.Managers.UnitsManagement
{
    public class Unit : IUnit
    {
        public IUnitStats Stats { get; private set; }
        public IUnitSkills Skills { get; private set; }
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

        public void HealHP(int Amount)
        {
            var afterHealAmount = CurrentHP + Amount;
            CurrentHP = afterHealAmount > Stats.MaxHP? Stats.MaxHP : afterHealAmount;
        }

        public void HealMP(int Amount)
        {
            var afterHealAmount = CurrentHP + Amount;
            CurrentHP = afterHealAmount > Stats.MaxMP ? Stats.MaxMP : afterHealAmount;
        }

        public bool IsHpFull()
        {
            return CurrentHP == Stats.MaxHP;
        }

        public bool IsMpFull()
        {
            return CurrentMP == Stats.MaxMP;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;

            if (CurrentHP < 0)
            {
                CurrentHP = 0;
            }
        }
    }
}
