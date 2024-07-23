using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryModel : Model
    {
        public List<EquipmentData_ScriptableObject> CurrentlyEquippedGearData { get; private set; } = new();
        public ItemsGridModel TalismanItemsGridModel { get; private set; }
        public ItemsGridModel WeaponItemsGridModel { get; private set; }
        public ItemsGridModel ShieldItemsGridModel { get; private set; }
        public ItemsGridModel HelmetItemsGridModel { get; private set; }
        public ItemsGridModel ArmorItemsGridModel { get; private set; }
        public ItemsGridModel BootsItemsGridModel { get; private set; }
        public ItemsGridModel CarryItemsGridModel { get; private set; }
        public ItemsGridModel QuickItemsGridModel { get; private set; }

        public bool IsInventoryOpen { get; private set; }
        public int Strength { get; private set; }
        public int EquippedWeight { get; private set; }
        public int GoldAmount { get; private set; }

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

        public bool TryAutoPlace_Loot(LootModel lootModel)
        {
            //var specificItemsGridModel = GetItemsGridModel(lootModel.LootData.LootType);

            //if(specificItemsGridModel.TryAutoPlace_Loot(lootModel))
            //{
            //    TryAddLootToCurrentlyEquippedGearList(specificItemsGridModel.ItemsGridType, lootModel);
            //    return true;
            //}

            return CarryItemsGridModel.TryAutoPlace_Loot(lootModel);
        }

        public bool TryAutoPlace_Gold(LootModel lootModel)
        {
            var isSuccess = CarryItemsGridModel.TryAutoPlace_Gold(lootModel);

            if(isSuccess)
            {
                SetGoldAmount();
            }

            return isSuccess;
        }

        public (bool, LootModel) TryHandPlaceOnGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            (bool, LootModel) resultTuple;
            var isGold = lootModel.LootCategory == LootCategory.Gold;

            if(isGold)
                resultTuple = itemsGridModel.TryHandPlace_Gold(tileIndex, lootModel);
            else
                resultTuple = itemsGridModel.TryHandPlace_Loot(tileIndex, lootModel);

            var isLootPlaced = resultTuple.Item1;
            var swappedLoot = resultTuple.Item2;

            if(isLootPlaced)
            {
                TryAddLootToCurrentlyEquippedGearList(itemsGridModel.ItemsGridType, lootModel);
            }

            if(lootModel.LootCategory == LootCategory.Gold || (swappedLoot != null && swappedLoot.LootCategory == LootCategory.Gold))
            {
                SetGoldAmount();
            }

            if(swappedLoot != null)
            {
                TryRemoveLootFromCurrentlyEquippedGearList(itemsGridModel.ItemsGridType, swappedLoot);
            }

            return resultTuple;
        }

        public void RemoveItemFromGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex)
        {
            var removedLootModel = itemsGridModel.RemoveItemFromGrid(tileIndex, true);
            TryRemoveLootFromCurrentlyEquippedGearList(itemsGridModel.ItemsGridType, removedLootModel);
            SetGoldAmount();
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

        public bool IsEquippableItemsGrid(ItemsGridType itemsGridType)
        {
            return (itemsGridType != ItemsGridType.Carry && itemsGridType != ItemsGridType.QuickItems);
        }

        public void SetStrengthAndEquippedWeight(int strength, int equippedWeight)
        {
            Strength = strength;
            EquippedWeight = equippedWeight;
            Changed();
        }

        private void SetGoldAmount()
        {
            var goldLootModels = CarryItemsGridModel.GetHeldGoldLootModels();
            int amount = 0;

            foreach(var model in goldLootModels)
            {
                amount += model.Amount;
            }

            GoldAmount = amount;
            Changed();
        }

        private bool TryAddLootToCurrentlyEquippedGearList(ItemsGridType itemsGridType, LootModel lootModel)
        {
            if(!IsEquippableItemsGrid(itemsGridType))
                return false;

            var equipmentData = lootModel.LootData as EquipmentData_ScriptableObject;
            CurrentlyEquippedGearData.Add(equipmentData);
            return true;
        }

        private bool TryRemoveLootFromCurrentlyEquippedGearList(ItemsGridType itemsGridType, LootModel lootModel)
        {
            if(!IsEquippableItemsGrid(itemsGridType))
                return false;

            var equipmentData = lootModel.LootData as EquipmentData_ScriptableObject;
            CurrentlyEquippedGearData.Remove(equipmentData);
            return true;
        }

        private ItemsGridModel GetItemsGridModel(LootType lootType)
        {
            switch(lootType)
            {
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
                    return CarryItemsGridModel;
            }
        }

        #endregion
    }
}


