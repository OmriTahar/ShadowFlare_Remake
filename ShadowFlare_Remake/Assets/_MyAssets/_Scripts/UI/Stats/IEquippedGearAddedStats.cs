namespace ShadowFlareRemake.UI.Stats
{
    public interface IEquippedGearAddedStats
    {
        public int MaxHP { get; }
        public int Attack { get; }
        public int Defense { get; }
        public int HitRate { get; }
        public int EvasionRate { get; }
        public int MovementSpeed { get; }
        public int AttackSpeed { get; }
        public int Strength { get; }
        public int EquippedWeight { get; }
        public int MaxMP { get; }
        public int MagicalAttack { get; }
        public int MagicalDefense { get; }
        public int MagicalHitRate { get; }
        public int MagicalEvasionRate { get; }
        public int MagicalAttackSpeed { get; }
    }
}
