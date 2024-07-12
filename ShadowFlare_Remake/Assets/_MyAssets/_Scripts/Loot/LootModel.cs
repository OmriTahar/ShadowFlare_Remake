using ShadowFlareRemake.Enums;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootModel : Model
    {
        public LootData_ScriptableObject LootData { get; private set; }
        public LootCategory LootCategory { get; private set; }
        public Color Color { get; private set; }
        public Color HighlightColor { get; private set; }
        public bool IsSingleTile { get; private set; } = false;
        public bool IsInvokeDropAnimation { get; private set; }

        private const float _highlightAdder = 0.2f;
        private const float _alpha = 1;

        public LootModel(LootData_ScriptableObject lootData)
        {
            LootData = lootData;
            Color = new Color(lootData.Color.r, lootData.Color.g, lootData.Color.b, 1);
            HighlightColor = new Color(Color.r + _highlightAdder, Color.g + _highlightAdder, Color.b + _highlightAdder, _alpha);
            SetLootCategory(lootData.LootType);
        }

        public void SetIsSingleTile(bool isSingleTile)
        {
            IsSingleTile = isSingleTile;
            Changed();
        }

        public void InvokeDropAnimation()
        {
            IsInvokeDropAnimation = true;
            Changed();
            IsInvokeDropAnimation = false;
        }

        private void SetLootCategory(LootType lootType)
        {
            if(IsGear(lootType))
            {
                LootCategory = LootCategory.Equipment;
            }
            else if(lootType == LootType.HealthPotion || lootType == LootType.ManaPotion)
            {
                LootCategory = LootCategory.Potion;
            }
            else if(lootType == LootType.Gold)
            {
                LootCategory = LootCategory.Gold;
            }
        }

        private bool IsGear(LootType lootType)
        {
            return (lootType == LootType.Weapon || lootType == LootType.Shield || lootType == LootType.Helmet ||
                    lootType == LootType.Armor || lootType == LootType.Boots || lootType == LootType.Talisman);
        }
    }
}
