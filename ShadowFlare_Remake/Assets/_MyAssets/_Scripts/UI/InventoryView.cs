using UnityEngine;

namespace ShadowFlareRemake.UI {
    public class InventoryView : UIView<InventoryModel> {

        [Header("References")]
        [SerializeField] private GameObject _inventoryPanel;

        protected override void ModelChanged() {
            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }
    }
}
