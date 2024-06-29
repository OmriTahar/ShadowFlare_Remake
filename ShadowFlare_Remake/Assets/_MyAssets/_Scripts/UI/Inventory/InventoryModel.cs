using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryModel : Model
    {
        public ItemsGridModel TalismanItemsGridModel { get; private set; }
        public ItemsGridModel WeaponItemsGridModel { get; private set; }
        public ItemsGridModel ShieldItemsGridModel { get; private set; }
        public ItemsGridModel HelmetItemsGridModel { get; private set; }
        public ItemsGridModel ArmorItemsGridModel { get; private set; }
        public ItemsGridModel BootsItemsGridModel { get; private set; }
        public ItemsGridModel CarryItemsGridModel { get; private set; }
        public ItemsGridModel QuickItemsGridModel { get; private set; }
        public bool IsInventoryOpen { get; private set; }

        private readonly Vector2Int _emptyTileIndex = new Vector2Int(-1, -1);

        #region Initialization

        public InventoryModel(bool isInventoryOpen)
        {
            SetIsInventoryOpen(isInventoryOpen);
            SetItemsGridModels();
        }

        private void SetItemsGridModels()
        {
            TalismanItemsGridModel = new ItemsGridModel("Talisman Grid", 2, 2, new List<LootType>() { LootType.Talisman });
            WeaponItemsGridModel = new ItemsGridModel("Weapon Grid", 1, 1, new List<LootType>() { LootType.Weapon });
            ShieldItemsGridModel = new ItemsGridModel("Shield Grid", 1, 1, new List<LootType>() { LootType.Shield });
            HelmetItemsGridModel = new ItemsGridModel("Helmet Grid", 1, 1, new List<LootType>() { LootType.Helmet });
            ArmorItemsGridModel = new ItemsGridModel("Armor Grid", 1, 1, new List<LootType>() { LootType.Armor });
            BootsItemsGridModel = new ItemsGridModel("Boots Grid", 1, 1, new List<LootType>() { LootType.Boots });
            CarryItemsGridModel = new ItemsGridModel("Carry Grid", 10, 4, new List<LootType>() { LootType.All }, false);
            QuickItemsGridModel = new ItemsGridModel("Quick Items Grid", 4, 2, new List<LootType>() { LootType.HealthPotion, LootType.ManaPotion });
        }

        #endregion

        #region Meat & Potatos

        public void SetIsInventoryOpen(bool isInventoryOpen)
        {
            IsInventoryOpen = isInventoryOpen;
            Changed();
        }

        public ItemsGridModel GetItemsGridModel(LootType lootType)
        {
            switch(lootType)
            {
                case LootType.All:
                    return CarryItemsGridModel;

                case LootType.Weapon:
                    return WeaponItemsGridModel;

                case LootType.Shield:
                    return ShieldItemsGridModel;

                case LootType.Helmet:
                    return HelmetItemsGridModel;

                case LootType.Armor:
                    return ArmorItemsGridModel;

                case LootType.Boots:
                    return BootsItemsGridModel;

                case LootType.Talisman:
                    return TalismanItemsGridModel;

                case LootType.HealthPotion:
                    return QuickItemsGridModel;

                case LootType.ManaPotion:
                    return QuickItemsGridModel;

                default:
                    return null;
            }
        }

        public LootModel GetQuickItemLootModel(Vector2Int index)
        {
            return QuickItemsGridModel.GetLootModelFromTileIndex(index);
        }

        public void RemovePotionFromInventory(Vector2Int index, LootType lootType)
        {
            var lootIndex = CarryItemsGridModel.GetLootModelRootIndexByType(lootType);

            if(lootIndex != _emptyTileIndex)
            {
                CarryItemsGridModel.RemoveItemFromGrid(lootIndex, true);
                return;
            }

            QuickItemsGridModel.RemoveItemFromGrid(index, true);
        }

        #endregion
    }
}


