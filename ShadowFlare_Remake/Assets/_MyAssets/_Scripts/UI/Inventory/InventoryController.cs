using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory {
    public class InventoryController : MonoBehaviour {

        //[Header("References")]
        //[SerializeField] private PlayerInput _playerInput;

        [Tooltip("Caches automatically through 'Detect Hover On Items Grid'")]
        public ItemsGrid CurrentHoveredItemsGrid;

        private void OnEnable() {
            //PlayerController.OnLeftMouseButtonClicked += SetCurrentHoveredItemsGrid;
        }

        private void OnDisable() {
            //PlayerController.OnLeftMouseButtonClicked -= SetCurrentHoveredItemsGrid;
        }

        private void SetCurrentHoveredItemsGrid() {

            if(CurrentHoveredItemsGrid == null)
                return;

            //print(CurrentHoveredItemsGrid.name + " was pressed at: " + CurrentHoveredItemsGrid.GetTileGridPosition(Mouse.current.position.ReadValue()));
        }
    }
}


