
namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryModel : Model
    {
        public ItemsGridModel WeaponPanelModel { get; private set; }
        public ItemsGridModel ShieldPanelModel { get; private set; }
        public ItemsGridModel HelmetPanelModel { get; private set; }
        public ItemsGridModel ArmorPanelModel { get; private set; }
        public ItemsGridModel BootsPanelModel { get; private set; }
        public ItemsGridModel CarryPanelModel { get; private set; }

        public bool IsInventoryOpen { get; private set; }

        public InventoryModel(bool isInventoryOpen)
        {
            SetIsInventoryOpen(isInventoryOpen);
            SetItemsGridModels();
        }

        private void SetItemsGridModels()
        {
            WeaponPanelModel = new ItemsGridModel("Weapon Panel", 2, 4);
            ShieldPanelModel = new ItemsGridModel("Shield Panel", 2, 3);
            HelmetPanelModel = new ItemsGridModel("Helmet Panel", 2,2);
            ArmorPanelModel = new ItemsGridModel("Armor Panel", 2,3);
            BootsPanelModel = new ItemsGridModel("Boots Panel", 2,2);
            CarryPanelModel = new ItemsGridModel("Carry Panel", 10, 4);
        }

        public void SetIsInventoryOpen(bool isInventoryOpen)
        {
            IsInventoryOpen = isInventoryOpen;
            Changed();
        }

        //public void SetCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel)
        //{
        //    CurrentHoveredItemsGridModel = itemsGridModel;
        //}

        //public void PickUpLoot(Vector2Int tileIndex, LootView lootView)
        //{
        //    PickedLoot = lootView;

        //    if(lootView == null)
        //    {
        //        PickedLootTransform = null;
        //        return;
        //    }

        //    PickedLootTransform = lootView.transform;
        //    CurrentHoveredItemsGridModel.RemoveItemFromGrid(tileIndex);
        //}

        //public void PlaceLootOnGrid(Vector2Int tileIndex, LootView lootView)
        //{
        //    CurrentHoveredItemsGridModel.PlaceLootOnGrid(tileIndex, lootView);

        //    PickedLoot = null;
        //    PickedLootTransform = null;
        //}

        //public void PlaceItem(Vector2Int tileIndex)
        //{
        //    if(!IsValidPlacement(PickedItem, tileIndex))
        //    {
        //        return;
        //    }

        //    ItemsDict[tileIndex] = PickedItem;

        //    //var isIndexTaken = ItemsDict.TryGetValue(tileIndex, out var carriedItem);

        //    //if(!isIndexTaken) {
        //    //    ItemsDict.Add(tileIndex, item);

        //    //} else {
        //    //    ItemsDict[tileIndex] = item;
        //    //}

        //    Changed();
        //}

    }
}


