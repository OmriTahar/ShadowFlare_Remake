using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public interface IEnemyUnitStats : IUnitStats {

        public Color Color { get; }
        public int ExpDrop { get; }
        public int CoinsDrop { get; }
    }
}
