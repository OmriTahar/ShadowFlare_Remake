using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Highlightables
{
    public class HighlightableData
    {
        // -------------- ALL ---------------- 
        public EntityType EntityType { get; private set; }
        public string Name { get; private set; }
        public Vector2Int NameBubbleOffset { get; private set; }

        // ------------- ENEMIES --------------- 
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int NameBgSize { get; private set; } = 1;
        public float ScaleMultiplier { get; private set; } = 1;

        // -------------- LOOT ----------------
        public LootCategory LootCategory { get; private set; }
        public int GoldAmount { get; private set; }

        public HighlightableData(EntityType entityType, string name, Vector2Int nameBubbleOffset)
        {
            EntityType = entityType;
            Name = name;
            NameBubbleOffset = nameBubbleOffset;
        }

        public HighlightableData(EntityType entityType, string name, Vector2Int nameBubbleOffset,
                                 int currentHP, int maxHP, int evolutionLevel, float scaleMultiplier)
        {
            EntityType = entityType;
            Name = name;
            NameBubbleOffset = nameBubbleOffset;
            CurrentHP = currentHP;
            MaxHP = maxHP;
            NameBgSize = evolutionLevel;
            ScaleMultiplier = scaleMultiplier;
        }

        public HighlightableData(EntityType entityType, string name, Vector2Int nameBubbleOffset,
                                 LootCategory lootCategory, int goldAmount)
        {
            EntityType = entityType;
            Name = name;
            NameBubbleOffset = nameBubbleOffset;
            LootCategory = lootCategory;
            GoldAmount = goldAmount;
        }
    }
}
