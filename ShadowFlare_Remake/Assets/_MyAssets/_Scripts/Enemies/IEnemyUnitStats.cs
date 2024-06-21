using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public interface IEnemyUnitStats : IUnitStats {

        public Color Color { get; }
        public float Scale { get; }
        public int EvolutionLevel { get; }
        public int ExpDrop { get; }
        public int CoinsDrop { get; }
        public float AttackDistance { get; }
    }
}
