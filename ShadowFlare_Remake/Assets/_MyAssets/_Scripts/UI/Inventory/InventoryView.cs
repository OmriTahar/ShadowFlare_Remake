using System;
using UnityEngine;
using ShadowFlareRemake.Loot;

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

        protected override void Clean()
        {
            DeregisterEvents();
        }

        #endregion


        #region Meat & Potatos

        private void InvokeCursorChangedHoverOverGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            OnCursorChangedHoverOverGrid?.Invoke(itemsGridModel, isCursorOn);
        }

        public void TileClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            OnTileClicked?.Invoke(itemsGridModel, tileIndex, lootModel);
        }

        public LootModel GetQuickItemLootModel(Vector2Int index)
        {
            return Model.GetQuickItemLootModel(index);
        }

        public void RemovePotionFromInventory(Vector2Int index)
        {
            Model.RemovePotionFromInventory(index);
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

            _talismanItemsGridView.OnTileClicked += TileClicked;
            _weaponItemsGridView.OnTileClicked += TileClicked;
            _shieldItemsGridlView.OnTileClicked += TileClicked;
            _helmetItemsGridView.OnTileClicked += TileClicked;
            _armorItemsGridView.OnTileClicked += TileClicked;
            _bootsItemsGridView.OnTileClicked += TileClicked;
            _carryItemsGridView.OnTileClicked += TileClicked;
            _quickItemsGridView.OnTileClicked += TileClicked;
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

            _talismanItemsGridView.OnTileClicked -= TileClicked;
            _weaponItemsGridView.OnTileClicked -= TileClicked;
            _shieldItemsGridlView.OnTileClicked -= TileClicked;
            _helmetItemsGridView.OnTileClicked -= TileClicked;
            _armorItemsGridView.OnTileClicked -= TileClicked;
            _bootsItemsGridView.OnTileClicked -= TileClicked;
            _carryItemsGridView.OnTileClicked -= TileClicked;
            _quickItemsGridView.OnTileClicked -= TileClicked;
        }

        #endregion
    }
}
