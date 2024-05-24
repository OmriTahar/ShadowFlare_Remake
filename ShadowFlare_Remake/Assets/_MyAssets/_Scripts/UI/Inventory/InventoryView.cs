using ShadowFlareRemake.Loot;
using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryView : UIView<InventoryModel>
    {
        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;
        public event Action<Vector2Int, LootView> OnTileClicked;

        [Header("References")]
        [SerializeField] private GameObject _inventoryPanel;
        [field: SerializeField] public LootView _testLootPrefab { get; private set; }

        [Header("Items Grid")]
        [SerializeField] private ItemsGridView _carryPanelView;

        private ItemsGridModel _carryPanelModel;

        protected override void Initialize()
        {
            RegisterEvents();
            SetCarryPanelModel();
        }

        protected override void ModelChanged()
        {
            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }

        private void SetCarryPanelModel()
        {
            _carryPanelView.SetModel(Model.CarryPanelModel);
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        private void RegisterEvents()
        {
            _carryPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _carryPanelView.OnTileClicked += TileClicked;
        }

        private void DeregisterEvents()
        {
            _carryPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _carryPanelView.OnTileClicked -= TileClicked;
        }

        private void InvokeCursorChangedHoverOverGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            OnCursorChangedHoverOverGrid?.Invoke(itemsGridModel, isCursorOn);
        }

        public void TileClicked(Vector2Int tileIndex, LootView lootView)
        {
            OnTileClicked?.Invoke(tileIndex, lootView);
        }
    }
}
