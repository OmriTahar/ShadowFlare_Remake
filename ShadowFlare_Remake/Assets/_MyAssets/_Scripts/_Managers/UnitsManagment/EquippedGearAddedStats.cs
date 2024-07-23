using ShadowFlareRemake.Loot;
using ShadowFlareRemake.UI.Stats;

namespace ShadowFlareRemake.Managers.UnitsManagement
{
    public class EquippedGearAddedStats : IEquippedGearAddedStats
    {
        public int MaxHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int HitRate { get; private set; }
        public int EvasionRate { get; private set; }
        public int MovementSpeed { get; private set; }
        public int AttackSpeed { get; private set; }
        public int Strength { get; private set; }
        public int EquippedWeight { get; private set; }
        public int MaxMP { get; private set; }
        public int MagicalAttack { get; private set; }
        public int MagicalDefense { get; private set; }
        public int MagicalHitRate { get; private set; }
        public int MagicalEvasionRate { get; private set; }
        public int MagicalAttackSpeed { get; private set; }

        public EquippedGearAddedStats() { }

        public void ResetValues()
        {
            MaxHP = 0;
            Attack = 0;
            Defense = 0;
            HitRate = 0;
            EvasionRate = 0;
            MovementSpeed = 0;
            AttackSpeed = 0;
            Strength = 0;
            EquippedWeight = 0;
            MaxMP = 0;
            MagicalAttack = 0;
            MagicalDefense = 0;
            MagicalHitRate = 0;
            MagicalEvasionRate = 0;
            MagicalAttackSpeed = 0;
        }

        public void AddEquippedGearStats(EquipmentData_ScriptableObject addedStats)
        {
            MaxHP += addedStats.MaxHP;
            Attack += addedStats.Attack;
            Defense += addedStats.Defense;
            HitRate += addedStats.HitRate;
            EvasionRate += addedStats.EvasionRate;
            MovementSpeed += addedStats.MovementSpeed;
            AttackSpeed += addedStats.AttackSpeed;
            Strength += addedStats.Strength;
            EquippedWeight += addedStats.Weight;
            MaxMP += addedStats.MaxMP;
            MagicalAttack += addedStats.MagicalAttack;
            MagicalDefense += addedStats.MagicalDefense;
            MagicalHitRate += addedStats.MagicalHitRate;
            MagicalEvasionRate += addedStats.MagicalEvasionRate;
            MagicalAttackSpeed += addedStats.MagicalAttackSpeed;
        }
    }
}
