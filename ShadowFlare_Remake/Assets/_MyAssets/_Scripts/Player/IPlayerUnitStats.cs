using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Player
{
    public interface IPlayerUnitStats : IUnitStats
    {
        public Vocation Vocation { get; }
        public int Strength { get; }
        public int EquippedWeight { get; }
        public int CurrentExp { get; }
        public int ExpToLevelUp { get; }
    }
}
