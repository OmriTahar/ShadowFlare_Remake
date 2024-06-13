using ShadowFlareRemake.Enums;

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

        public InventoryModel(bool isInventoryOpen)
        {
            SetIsInventoryOpen(isInventoryOpen);
            SetItemsGridModels();
        }

        private void SetItemsGridModels()
        {
            TalismanItemsGridModel = new ItemsGridModel("Talisman Grid", 2, 2, LootType.Talisman);
            WeaponItemsGridModel = new ItemsGridModel("Weapon Grid", 1, 1, LootType.Weapon);
            ShieldItemsGridModel = new ItemsGridModel("Shield Grid", 1, 1, LootType.Shield);
            HelmetItemsGridModel = new ItemsGridModel("Helmet Grid", 1, 1, LootType.Helmet);
            ArmorItemsGridModel = new ItemsGridModel("Armor Grid", 1, 1, LootType.Armor);
            BootsItemsGridModel = new ItemsGridModel("Boots Grid", 1, 1, LootType.Boots);
            CarryItemsGridModel = new ItemsGridModel("Carry Grid", 10, 4, LootType.All, false);
            QuickItemsGridModel = new ItemsGridModel("Quick Items Grid", 4, 2, LootType.Potion);
        }

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

                case LootType.Potion:
                    return QuickItemsGridModel;

                default:
                    return null;
            }
        }
    }
}


