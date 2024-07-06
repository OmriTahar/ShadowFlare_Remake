using ShadowFlareRemake.Loot;
using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryView : UIView<InventoryModel>
    {
        public event Action<LootModel, Vector2Int> OnTileHovered;
        public event Action<ItemsGridModel, Vector2Int, LootModel> OnTileClicked;
        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;

        [Header("References")]
        [SerializeField] private GameObject _inventoryPanel;

        [Header("Items Grids")]
        [SerializeField] private ItemsGridView _talismanItemsGridView;
        [SerializeField] private ItemsGridView _weaponItemsGridView;
        [SerializeField] private ItemsGridView _shieldItemsGridlView;
        [SerializeField] private ItemsGridView _helmetItemsGridView;
        [SerializeField] private ItemsGridView _armorItemsGridView;
        [SerializeField] private ItemsGridView _bootsItemsGridView;
        [SerializeField] private ItemsGridView _carryItemsGridView;
        [SerializeField] private ItemsGridView _quickItemsGridView;

        #region View Overrides

        protected override void Initialize()
        {
            RegisterEvents();
            SetItemsGridModels();
        }

        protected override void ModelChanged()
        {
            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        #endregion

        #region Initialization

        private void SetItemsGridModels()
        {
            _talismanItemsGridView.SetModel(Model.TalismanItemsGridModel);
            _weaponItemsGridView.SetModel(Model.WeaponItemsGridModel);
            _shieldItemsGridlView.SetModel(Model.ShieldItemsGridModel);
            _helmetItemsGridView.SetModel(Model.HelmetItemsGridModel);
            _armorItemsGridView.SetModel(Model.ArmorItemsGridModel);
            _bootsItemsGridView.SetModel(Model.BootsItemsGridModel);
            _carryItemsGridView.SetModel(Model.CarryItemsGridModel);
            _quickItemsGridView.SetModel(Model.QuickItemsGridModel);
        }

        #endregion

        #region Meat & Potatos

        private void InvokeCursorChangedHoverOverGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            OnCursorChangedHoverOverGrid?.Invoke(itemsGridModel, isCursorOn);
        }

        public void InvokeTileHovered(LootModel lootModel, Vector2Int index)
        {
            OnTileHovered?.Invoke(lootModel, index);
        }

        public void InvokeTileClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            OnTileClicked?.Invoke(itemsGridModel, tileIndex, lootModel);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _talismanItemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _weaponItemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _shieldItemsGridlView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _helmetItemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _armorItemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _bootsItemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _carryItemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _quickItemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;

            _talismanItemsGridView.OnTileClicked += InvokeTileClicked;
            _weaponItemsGridView.OnTileClicked += InvokeTileClicked;
            _shieldItemsGridlView.OnTileClicked += InvokeTileClicked;
            _helmetItemsGridView.OnTileClicked += InvokeTileClicked;
            _armorItemsGridView.OnTileClicked += InvokeTileClicked;
            _bootsItemsGridView.OnTileClicked += InvokeTileClicked;
            _carryItemsGridView.OnTileClicked += InvokeTileClicked;
            _quickItemsGridView.OnTileClicked += InvokeTileClicked;

            _carryItemsGridView.OnTileHovered += InvokeTileHovered;
            _quickItemsGridView.OnTileHovered += InvokeTileHovered;
        }

        private void DeregisterEvents()
        {
            _talismanItemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _weaponItemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _shieldItemsGridlView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _helmetItemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _armorItemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _bootsItemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _carryItemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _quickItemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;

            _talismanItemsGridView.OnTileClicked -= InvokeTileClicked;
            _weaponItemsGridView.OnTileClicked -= InvokeTileClicked;
            _shieldItemsGridlView.OnTileClicked -= InvokeTileClicked;
            _helmetItemsGridView.OnTileClicked -= InvokeTileClicked;
            _armorItemsGridView.OnTileClicked -= InvokeTileClicked;
            _bootsItemsGridView.OnTileClicked -= InvokeTileClicked;
            _carryItemsGridView.OnTileClicked -= InvokeTileClicked;
            _quickItemsGridView.OnTileClicked -= InvokeTileClicked;

            _carryItemsGridView.OnTileHovered -= InvokeTileHovered;
            _quickItemsGridView.OnTileHovered -= InvokeTileHovered;
        }

        #endregion
    }
}
