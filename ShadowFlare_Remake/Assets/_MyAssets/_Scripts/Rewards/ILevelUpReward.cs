namespace ShadowFlareRemake.Rewards {
    public interface ILevelUpReward {
        public int MaxHP { get; }
        public int MaxMP { get; }
        public int Strength { get; }
        public int Attack { get; }
        public int Defense { get; }
        public int MagicalAttack { get; }
        public int MagicalDefence { get; }
    }
}
