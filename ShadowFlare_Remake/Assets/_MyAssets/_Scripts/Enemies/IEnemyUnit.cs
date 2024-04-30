using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public interface IEnemyUnit : IUnit {

        public Color Color { get; }
        public int ExpDrop { get; }
        public int CoinsDrop { get; }
    }
}
