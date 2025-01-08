using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI.Highlightables
{
    public class HighlightableData
    {
        // -------------- ALL ---------------- 
        public EntityType EntityType { get; private set; }
        public string Name { get; private set; }
        public int UiOffset { get; private set; }

        // ------------- ENEMIES --------------- 
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int EvolutionLevel { get; private set; } = 1;
        public float ScaleMultiplier { get; private set; } = 1;

        // -------------- LOOT ----------------
        public LootCategory LootCategory { get; private set; }
        public int GoldAmount { get; private set; }

        public HighlightableData(EntityType entityType, string name, int uiOffset)
        {
            EntityType = entityType;
            Name = name;
            UiOffset = uiOffset;
        }

        public HighlightableData(EntityType entityType, string name, int uiOffset, 
                                 int currentHP, int maxHP, int evolutionLevel, float scaleMultiplier)
        {
            EntityType = entityType;
            Name = name;
            UiOffset = uiOffset;
            CurrentHP = currentHP;
            MaxHP = maxHP;
            EvolutionLevel = evolutionLevel;
            ScaleMultiplier = scaleMultiplier;
        }

        public HighlightableData(EntityType entityType, string name, int uiOffset, LootCategory lootCategory, int goldAmount)
        {
            EntityType = entityType;
            Name = name;
            UiOffset = uiOffset;
            LootCategory = lootCategory;
            GoldAmount = goldAmount;
        }
    }
}
