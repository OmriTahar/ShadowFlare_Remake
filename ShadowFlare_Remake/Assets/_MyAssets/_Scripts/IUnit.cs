namespace ShadowFlareRemake {

    public interface IUnit {
        public UnitStats Stats { get; }
        public int CurrentHP { get; }
        public int CurrentMP { get; }
    }
}
