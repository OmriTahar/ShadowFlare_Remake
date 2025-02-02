using ShadowFlareRemake.Loot;
using ShadowFlareRemake.UI.ItemsGrid;
using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Warehouse
{
    public class WarehouseView : UIView<WarehouseModel>
    {
        public event Action<LootModel, Vector2Int> OnTileHovered;
        public event Action<ItemsGridModel, Vector2Int, LootModel> OnTileClicked;
        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;

        [Header("References")]
        [SerializeField] private GameObject _warehousePanel;
        [SerializeField] private ItemsGridView _itemsGridView;

        #region View Overrides

        protected override void Initialize()
        {
            _itemsGridView.SetModel(Model.WarehouseItemsGridModel);
            RegisterEvents();
        }

        protected override void ModelChanged()
        {
            SetIsActive();
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

        public void InvokeTileHovered(LootModel lootModel, Vector2Int index)
        {
            OnTileHovered?.Invoke(lootModel, index);
        }

        public void InvokeTileClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            OnTileClicked?.Invoke(itemsGridModel, tileIndex, lootModel);
        }

        private void SetIsActive()
        {
            _warehousePanel.SetActive(Model.IsWarehouseActive);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _itemsGridView.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
            _itemsGridView.OnTileHovered += InvokeTileHovered;
            _itemsGridView.OnTileClicked += InvokeTileClicked;
        }

        private void DeregisterEvents()
        {
            _itemsGridView.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
            _itemsGridView.OnTileHovered -= InvokeTileHovered;
            _itemsGridView.OnTileClicked -= InvokeTileClicked;
        }

        #endregion
    }
}
