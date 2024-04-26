
namespace ShadowFlareRemake.UI {
    public class InventoryModel : Model {

        public bool IsInventoryOpen { get; private set; }

        public InventoryModel(bool isInventoryOpen) {

            UpdateInventory(isInventoryOpen);
        }

        public void UpdateInventory(bool isInventoryOpen) {

            IsInventoryOpen = isInventoryOpen;
            Changed();
        }
    }
}


