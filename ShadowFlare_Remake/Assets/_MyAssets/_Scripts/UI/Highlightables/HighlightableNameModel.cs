using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Highlightables
{
    public class HighlightableNameModel : Model
    {
        public string Name { get; private set; }
        public EntityType EntityType { get; private set; }
        public Transform CurrentEntityTransform { get; private set; }
        public int UiOffest { get; private set; }
        public bool IsActive { get; private set; }

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int EvolutionLevel { get; private set; }
        public float ScaleMultiplier { get; private set; }
        public LootCategory LootCategory {  get; private set; } 
        public int GoldAmount { get; private set; }

        public HighlightableNameModel()
        {
            EntityType = EntityType.None;
            Name = "";
            CurrentHP = 0;
            MaxHP = 0;
            EvolutionLevel = 1;
            UiOffest = 0;
            ScaleMultiplier = 1;
            CurrentEntityTransform = null;
        }

        public void SetHighlightableData(HighlightableData data, Transform entityTransform)
        {
            Name = data.Name;
            EntityType = data.EntityType;
            CurrentEntityTransform = entityTransform;
            UiOffest = data.UiOffset;
            CurrentHP = data.CurrentHP;
            MaxHP = data.MaxHP;
            EvolutionLevel = data.EvolutionLevel;
            ScaleMultiplier = data.ScaleMultiplier;
            LootCategory = data.LootCategory;
            GoldAmount = data.GoldAmount;
            Changed();
        }

        public void SetEntityTransform(Transform entityTransform)
        {
            CurrentEntityTransform = entityTransform;
            Changed();
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
            Changed();
        }
    }
}
