
namespace ShadowFlareRemake.UI.Inventory {
    public class InventoryModel : Model {

        public ItemsGridModel CurrentHoveredItemsGridModel { get; private set; }
        public bool IsInventoryOpen { get; private set; }

        public InventoryModel(bool isInventoryOpen) {

            SetIsInventoryOpen(isInventoryOpen);
        }

        public void SetIsInventoryOpen(bool isInventoryOpen) {

            IsInventoryOpen = isInventoryOpen;
            Changed();
        }

        public void SetCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel) {

            CurrentHoveredItemsGridModel = itemsGridModel;
        }

        public void PlaceItem(InventoryItem inventoryItem, int posX, int posY) {

            CurrentHoveredItemsGridModel.PlaceItem(inventoryItem, posX, posY);
        }
    }
}


