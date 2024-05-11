namespace ShadowFlareRemake.Rewards {
    public interface ILevelUpReward {
        public int HP { get; }
        public int MP { get; }
        public int Strength { get; }
        public int Attack { get; }
        public int Defense { get; }
        public int MagicalAttack { get; }
        public int MagicalDefence { get; }
    }
}
