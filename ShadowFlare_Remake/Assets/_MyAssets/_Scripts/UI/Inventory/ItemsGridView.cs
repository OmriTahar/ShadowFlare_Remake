using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI.Inventory
{
    public class ItemsGridView : View<ItemsGridModel>, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;
        public event Action<ItemsGridModel, Vector2Int, LootModel> OnTileClicked;

        private Dictionary<Vector2Int, GridTileView> _gridTilesDict = new();

        protected override void Initialize()
        {
            InitGridTilesDict();
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        protected override void ModelChanged()
        {
            PlaceItems();
        }

        private void InitGridTilesDict()
        {
            var gridTileViews = GetComponentsInChildren<GridTileView>();

            foreach(var tileView in gridTileViews)
            {
                _gridTilesDict.Add(tileView.Index, tileView);
                tileView.OnTileClicked += InovkeTileClicked;
            }
        }

        private void PlaceItems()
        {
            foreach(var keyValuePair in Model.GridTileModelsDict)
            {
                var index = keyValuePair.Key;
                var gridTileModel = keyValuePair.Value;
                var lootModel = gridTileModel.LootModel;

                if(lootModel == null)
                    continue;

                var gridTileView = _gridTilesDict[index];
                gridTileView.SetModel(gridTileModel);
            }
        }

        private void InovkeTileClicked(Vector2Int index)
        {
            var lootModel = Model.GetLootModelFromTileIndex(index);
            OnTileClicked?.Invoke(Model, index, lootModel);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnCursorChangedHoverOverGrid?.Invoke(Model, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnCursorChangedHoverOverGrid?.Invoke(Model, false);
        }

        private void DeregisterEvents()
        {
            foreach(var tileView in _gridTilesDict.Values)
            {
                tileView.OnTileClicked -= InovkeTileClicked;
            }
        }
    }
}

