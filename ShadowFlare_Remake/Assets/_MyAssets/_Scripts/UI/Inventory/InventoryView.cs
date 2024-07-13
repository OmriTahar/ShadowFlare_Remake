using ShadowFlareRemake.Loot;
using System;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private TMP_Text _goldText;

        [Header("Items Grids")]
        [SerializeField] private ItemsGridView _talismanItemsGridView;
        [SerializeField] private ItemsGridView _weaponItemsGridView;
        [SerializeField] private ItemsGridView _shieldItemsGridlView;
        [SerializeField] private ItemsGridView _helmetItemsGridView;
        [SerializeField] private ItemsGridView _armorItemsGridView;
        [SerializeField] private ItemsGridView _bootsItemsGridView;
        [SerializeField] private ItemsGridView _carryItemsGridView;
        [SerializeField] private ItemsGridView _quickItemsGridView;

        private List<ItemsGridView> _itemsGridViews = new();

        #region View Overrides

        protected override void Initialize()
        {
            InitItemsGridViewsList();
            RegisterEvents();
            SetItemsGridModels();
        }

        protected override void ModelChanged()
        {
            SetGoldAmount();
            SetIsActive();
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        #endregion

        #region Initialization

        private void InitItemsGridViewsList()
        {
            _itemsGridViews.Add(_talismanItemsGridView);
            _itemsGridViews.Add(_weaponItemsGridView);
            _itemsGridViews.Add(_shieldItemsGridlView);
            _itemsGridViews.Add(_helmetItemsGridView);
            _itemsGridViews.Add(_armorItemsGridView);
            _itemsGridViews.Add(_bootsItemsGridView);
            _itemsGridViews.Add(_carryItemsGridView);
            _itemsGridViews.Add(_quickItemsGridView);
        }

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

        private void SetGoldAmount()
        {
            _goldText.text = Model.GoldAmount.ToString();
        }

        private void SetIsActive()
        {
            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            foreach(var view in _itemsGridViews)
            {
                view.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
                view.OnTileHovered += InvokeTileHovered;
                view.OnTileClicked += InvokeTileClicked;
            }
        }

        private void DeregisterEvents()
        {
            foreach(var view in _itemsGridViews)
            {
                view.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
                view.OnTileHovered -= InvokeTileHovered;
                view.OnTileClicked -= InvokeTileClicked;
            }
        }

        #endregion
    }
}
