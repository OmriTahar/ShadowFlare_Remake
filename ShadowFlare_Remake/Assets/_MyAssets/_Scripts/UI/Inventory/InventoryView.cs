using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory {
    public class InventoryView : UIView<InventoryModel> {

        public event Action<ItemsGridView, bool> OnCursorChangedHoverOverGrid;
        public event Action<Vector2Int> OnTileClicked;

        [Header("References")]
        [SerializeField] private GameObject _inventoryPanel;
        [field: SerializeField] public InventoryItem _inventoryItemPrefab { get; private set; }

        [Header("Items Grid")]
        [SerializeField] private ItemsGridView _carryPanelView;

        private ItemsGridModel _carryPanelModel;

        protected override void Initialize() {

            RegisterEvents();
            SetCarryPanelModel();
        }

        protected override void ModelChanged() {

            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }

        private void SetCarryPanelModel() {

            _carryPanelModel = new ItemsGridModel(80, 80);
            _carryPanelView.SetModel(_carryPanelModel);
        }

        protected override void Clean() {

            DeregisterEvents();
        }

        private void RegisterEvents() {

            _carryPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _carryPanelView.OnTileClicked += TileClicked;
        }

        private void DeregisterEvents() {

            _carryPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _carryPanelView.OnTileClicked -= TileClicked;
        }

        private void InvokeCursorChangedHoverOverGrid(ItemsGridView itemsGrid, bool isCursorOn) {

            OnCursorChangedHoverOverGrid?.Invoke(itemsGrid, isCursorOn);
        }

        public void TileClicked(Vector2Int tileClicked) {

            OnTileClicked?.Invoke(tileClicked);
        }
    }
}
