using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryModel : Model
    {
        public InventoryItem PickedItem { get; private set; }
        public Transform PickedItemTransform { get; private set; }

        public ItemsGridModel CurrentHoveredItemsGridModel { get; private set; }
        public bool IsInventoryOpen { get; private set; }

        public InventoryModel(bool isInventoryOpen)
        {
            SetIsInventoryOpen(isInventoryOpen);
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

        public void PickUpItem(Vector2Int tileIndex, InventoryItem item)
        {
            PickedItem = item;

            if(item == null)
            {
                PickedItemTransform = null;
                return;
            }

            PickedItemTransform = item.transform;
            CurrentHoveredItemsGridModel.RemoveItemFromGrid(tileIndex);
        }

        public void PlaceItem(Vector2Int tileIndex, InventoryItem item)
        {
            CurrentHoveredItemsGridModel.PlaceItemOnGrid(tileIndex, item);

            PickedItem = null;
            PickedItemTransform = null;
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


