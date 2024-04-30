using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Player {

    public interface IPlayerUnit : IUnit {

        public Vocation Vcocation { get; }
        public int Strength { get; }
        public int CurrentExp { get; }
        public int ExpToLevelUp { get; }
    }
}
