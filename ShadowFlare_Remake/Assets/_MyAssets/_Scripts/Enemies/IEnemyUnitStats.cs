using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Enemies
{
    public interface IEnemyUnitStats : IUnitStats
    {
        public Color Color { get; }
        public float ScaleMultiplier { get; }
        public int EvolutionLevel { get; }
        public float AttackDistance { get; }
        public int ChaseDistance { get; }
        public int ExpReward { get; }
        public int LootDropChance { get; }
    }
}
