using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryModel : Model
    {
        public LootView PickedLoot { get; private set; }
        public Transform PickedLootTransform { get; private set; }
        public ItemsGridModel CurrentHoveredItemsGridModel { get; private set; }

        public ItemsGridModel CarryPanelModel { get; private set; }

        public bool IsInventoryOpen { get; private set; }

        public InventoryModel(bool isInventoryOpen)
        {
            SetIsInventoryOpen(isInventoryOpen);
            CarryPanelModel = new ItemsGridModel("Carry Panel", 80, 80);
        }

        public void SetIsInventoryOpen(bool isInventoryOpen)
        {
            IsInventoryOpen = isInventoryOpen;
            Changed();
        }

        public void SetCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel)
        {
            CurrentHoveredItemsGridModel = itemsGridModel;
        }

        public void PickUpLoot(Vector2Int tileIndex, LootView lootView)
        {
            PickedLoot = lootView;

            if(lootView == null)
            {
                PickedLootTransform = null;
                return;
            }

            PickedLootTransform = lootView.transform;
            CurrentHoveredItemsGridModel.RemoveItemFromGrid(tileIndex);
        }

        public void PlaceLootOnGrid(Vector2Int tileIndex, LootView lootView)
        {
            CurrentHoveredItemsGridModel.PlaceLootOnGrid(tileIndex, lootView);

            PickedLoot = null;
            PickedLootTransform = null;
        }

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

