using ShadowFlareRemake.GameManager.Units;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.GameManager
{
    public class EquippedGearAddedStats : IEquippedGearAddedStats
    {
        public int Strength { get; private set; }
        public int MaxHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int HitRate { get; private set; }
        public int EvasionRate { get; private set; }
        public int MovementSpeed { get; private set; }
        public int AttackSpeed { get; private set; }
        public int MaxMP { get; private set; }
        public int MagicalAttack { get; private set; }
        public int MagicalDefense { get; private set; }
        public int MagicalHitRate { get; private set; }
        public int MagicalEvasionRate { get; private set; }
        public int MagicalAttackSpeed { get; private set; }

        public EquippedGearAddedStats() { }

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
            MaxMP += addedStats.MaxMP;
            MagicalAttack += addedStats.MagicalAttack;
            MagicalDefense += addedStats.MagicalDefense;
            MagicalHitRate += addedStats.MagicalHitRate;
            MagicalEvasionRate += addedStats.MagicalEvasionRate;
            MagicalAttackSpeed += addedStats.MagicalAttackSpeed;
        }
    }
}
