namespace ShadowFlareRemake {

    public interface IUnit {
        public IUnitStats Stats { get; }
        public int CurrentHP { get; }
        public int CurrentMP { get; }
    }
}
