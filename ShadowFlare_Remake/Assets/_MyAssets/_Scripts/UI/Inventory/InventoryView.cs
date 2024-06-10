using ShadowFlareRemake.Loot;
using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryView : UIView<InventoryModel>
    {
        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;
        public event Action<ItemsGridModel, Vector2Int, LootModel> OnTileClicked;

        [Header("References")]
        [SerializeField] private GameObject _inventoryPanel;
        [field: SerializeField] public LootView _testLootPrefab { get; private set; }

        [Header("Items Grid")]
        [SerializeField] private ItemsGridView _weaponPanelView;
        [SerializeField] private ItemsGridView _shieldPanelView;
        [SerializeField] private ItemsGridView _helmetPanelView;
        [SerializeField] private ItemsGridView _armorPanelView;
        [SerializeField] private ItemsGridView _bootsPanelView;
        [SerializeField] private ItemsGridView _carryPanelView;

        protected override void Initialize()
        {
            RegisterEvents();
            SetItemsGridModels();
        }

        protected override void ModelChanged()
        {
            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }

        private void SetItemsGridModels()
        {
            _weaponPanelView.SetModel(Model.WeaponPanelModel);
            _shieldPanelView.SetModel(Model.ShieldPanelModel);
            _helmetPanelView.SetModel(Model.HelmetPanelModel);
            _armorPanelView.SetModel(Model.ArmorPanelModel);
            _bootsPanelView.SetModel(Model.BootsPanelModel);
            _carryPanelView.SetModel(Model.CarryPanelModel);
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        private void RegisterEvents()
        {
            _weaponPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _shieldPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _helmetPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _armorPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _bootsPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _carryPanelView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;

            _weaponPanelView.OnTileClicked += TileClicked;
            _shieldPanelView.OnTileClicked += TileClicked;
            _helmetPanelView.OnTileClicked += TileClicked;
            _armorPanelView.OnTileClicked += TileClicked;
            _bootsPanelView.OnTileClicked += TileClicked;
            _carryPanelView.OnTileClicked += TileClicked;
        }

        private void DeregisterEvents()
        {
            _weaponPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _shieldPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _helmetPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _armorPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _bootsPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _carryPanelView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;

            _weaponPanelView.OnTileClicked -= TileClicked;
            _shieldPanelView.OnTileClicked -= TileClicked;
            _helmetPanelView.OnTileClicked -= TileClicked;
            _armorPanelView.OnTileClicked -= TileClicked;
            _bootsPanelView.OnTileClicked -= TileClicked;
            _carryPanelView.OnTileClicked -= TileClicked;
        }

        private void InvokeCursorChangedHoverOverGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            OnCursorChangedHoverOverGrid?.Invoke(itemsGridModel, isCursorOn);
        }

        public void TileClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            OnTileClicked?.Invoke(itemsGridModel, tileIndex, lootModel);
        }
    }
}
