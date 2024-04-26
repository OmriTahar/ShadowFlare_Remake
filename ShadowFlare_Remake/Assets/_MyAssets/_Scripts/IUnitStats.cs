namespace ShadowFlareRemake {
    public interface IUnitStats {

        public string Name { get; }
        public int Level { get; }

        public int MaxHP { get; }
        public int Strength { get; }
        public int Attack { get; }
        public int Defence { get; }
        public int HitRate { get; }
        public int EvasionRate { get; }
        public int WalkingSpeed { get; }
        public int AttackSpeed { get; }

        public int MaxMP { get; }
        public int MagicalAttack { get; }
        public int MagicalDefence { get; }
        public int MagicalHitRate { get; }
        public int MagicalEvasionRate { get; }
        public int MagicalAttackSpeed { get; }
    }
}
