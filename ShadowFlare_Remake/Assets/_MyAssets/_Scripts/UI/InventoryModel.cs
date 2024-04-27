
namespace ShadowFlareRemake.UI {
    public class InventoryModel : Model {

        public bool IsInventoryOpen { get; private set; }

        public InventoryModel(bool isInventoryOpen) {

            SetIsInventoryOpen(isInventoryOpen);
        }

        public void SetIsInventoryOpen(bool isInventoryOpen) {

            IsInventoryOpen = isInventoryOpen;
            Changed();
        }
    }
}


