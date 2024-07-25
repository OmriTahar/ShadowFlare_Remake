namespace ShadowFlareRemake.Units
{
    public interface IUnit
    {
        IUnitStats Stats { get; }
        public int CurrentHP { get; }
        public int CurrentMP { get; }
    }
}
