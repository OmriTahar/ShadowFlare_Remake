using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Enemies
{
    public interface IEnemyUnitStats : IUnitStats
    {
        public Color Color { get; }
        public float Scale { get; }
        public int EvolutionLevel { get; }
        public float AttackDistance { get; }
        public int ExpReward { get; }
        public int LootDropChance { get; }
    }
}
