using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;
using System.Collections.Generic;

namespace ShadowFlareRemake.UnitsRestrictedData
{
    public class Unit : IUnit
    {
        public IUnitStats Stats { get; private set; }
        public List<ISkillData> Skills { get; private set; }
        public int CurrentHP { get; private set; }
        public int CurrentMP { get; private set; }

        public Unit(IUnitStats stats, List<ISkillData> skills)
        {
            Stats = stats;
            Skills = skills;

            CurrentHP = stats.MaxHP;
            CurrentMP = stats.MaxMP;
        }

        public void FullHeal()
        {
            CurrentHP = Stats.MaxHP;
            CurrentMP = Stats.MaxMP;
        }

        public void RestoreHP(int amount)
        {
            var postRestoreAmount = CurrentHP + amount;
            CurrentHP = postRestoreAmount > Stats.MaxHP? Stats.MaxHP : postRestoreAmount;
        }

        public void ReduceHP(int amount)
        {
            CurrentHP -= amount;

            if(CurrentHP < 0)
            {
                CurrentHP = 0;
            }
        }

        public void RestoreMP(int amount)
        {
            var postRestoreAmount = CurrentMP + amount;
            CurrentMP = postRestoreAmount > Stats.MaxMP ? Stats.MaxMP : postRestoreAmount;
        }

        public void ReduceMP(int amount)
        {
            CurrentMP -= amount;

            if(CurrentMP < 0)
            {
                CurrentMP = 0;
            }
        }

        public bool IsHpFull()
        {
            return CurrentHP == Stats.MaxHP;
        }

        public bool IsMpFull()
        {
            return CurrentMP == Stats.MaxMP;
        }

    }
}
