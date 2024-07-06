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

        public List<LootModel> CurrentlyEquippedGear { get; private set; } = new();
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
            TalismanItemsGridModel = new ItemsGridModel(ItemsGridType.Talisman, 2, 2, new List<LootType>() { LootType.Talisman });
            WeaponItemsGridModel = new ItemsGridModel(ItemsGridType.Weapon, 1, 1, new List<LootType>() { LootType.Weapon });
            ShieldItemsGridModel = new ItemsGridModel(ItemsGridType.Shield, 1, 1, new List<LootType>() { LootType.Shield });
            HelmetItemsGridModel = new ItemsGridModel(ItemsGridType.Helmet, 1, 1, new List<LootType>() { LootType.Helmet });
            ArmorItemsGridModel = new ItemsGridModel(ItemsGridType.Armor, 1, 1, new List<LootType>() { LootType.Armor });
            BootsItemsGridModel = new ItemsGridModel(ItemsGridType.Boots, 1, 1, new List<LootType>() { LootType.Boots });
            CarryItemsGridModel = new ItemsGridModel(ItemsGridType.Carry, 10, 4, new List<LootType>() { LootType.All }, false);
            QuickItemsGridModel = new ItemsGridModel(ItemsGridType.QuickItems, 4, 2, new List<LootType>() { LootType.HealthPotion, LootType.ManaPotion });
        }

        #endregion

        #region Meat & Potatos

        public void SetIsInventoryOpen(bool isInventoryOpen)
        {
            IsInventoryOpen = isInventoryOpen;
            Changed();

        }

        public bool TryAutoPlaceLootOnGrid(LootModel lootModel)
        {
            var specificItemsGridModel = GetItemsGridModel(lootModel.LootData.LootType);

            if (specificItemsGridModel.TryAutoPlaceLootOnGrid(lootModel))
            {
                if(IsEquippableItemsGrid(specificItemsGridModel.ItemsGridType))
                {
                    CurrentlyEquippedGear.Add(lootModel);
                }

                return true;
            }

            return CarryItemsGridModel.TryAutoPlaceLootOnGrid(lootModel);
        }

        public (bool, LootModel) TryHandPlaceLootOnGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            var tuple = itemsGridModel.TryHandPlaceLootOnGrid(tileIndex, lootModel);
            var isLootPlaced = tuple.Item1;

            if(isLootPlaced)
            {
                CurrentlyEquippedGear.Add(lootModel);
            }

            return tuple;
        }

        public void RemoveItemFromGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex)
        {
            itemsGridModel.RemoveItemFromGrid(tileIndex, true);
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

        private ItemsGridModel GetItemsGridModel(LootType lootType)
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

        private bool IsEquippableItemsGrid(ItemsGridType itemsGridType)
        {
            return (itemsGridType != ItemsGridType.Carry && itemsGridType != ItemsGridType.QuickItems);
        }

        #endregion
    }
}


