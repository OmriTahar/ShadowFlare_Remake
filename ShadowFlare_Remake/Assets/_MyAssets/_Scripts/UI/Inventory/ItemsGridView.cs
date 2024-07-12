using ShadowFlareRemake.Loot;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI.Inventory
{
    public class ItemsGridView : View<ItemsGridModel>, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;
        public event Action<ItemsGridModel, Vector2Int, LootModel> OnTileClicked;
        public event Action<LootModel, Vector2Int> OnTileHovered;

        private Dictionary<Vector2Int, GridTileView> _gridTilesDict = new();

        [Header("References")]
        [SerializeField] private GameObject _placeHolderSprite;

        private readonly Vector2Int _emptyTileIndex = new Vector2Int(-1, -1);

        #region View Overrides

        protected override void Initialize()
        {
            InitGridTilesDict();
            RegisterEvents();
        }

        protected override void ModelChanged()
        {
            PlaceItems();
            HandlePlaceHolderSprite();
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        #endregion

        #region Initialization

        private void InitGridTilesDict()
        {
            var gridTileViews = GetComponentsInChildren<GridTileView>();

            foreach(var tileView in gridTileViews)
            {
                _gridTilesDict.Add(tileView.Index, tileView);
            }
        }

        #endregion

        #region Meat & Potatos

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

        private void HandlePlaceHolderSprite()
        {
            if(!Model.HasPlaceHolderSprite)
                return;

            if(Model.IsSingleTile)
            {
                var lootModel = Model.GridTileModelsDict[Vector2Int.zero].LootModel;
                _placeHolderSprite.SetActive(lootModel == null);
                return;
            }

            var isGridContainsLoot = Model.IsGridContainsLoot();
            _placeHolderSprite.SetActive(!isGridContainsLoot);
        }

        private void InovkeTileClicked(Vector2Int index)
        {
            var lootModel = Model.GetLootModelFromTileIndex(index);
            OnTileClicked?.Invoke(Model, index, lootModel);
        }

        private void InovkeGridTileHovered(Vector2Int index, bool isHovered)
        {
            if(!isHovered)
            {
                OnTileHovered?.Invoke(null, _emptyTileIndex);
                return;
            }

            var lootModel = Model.GetLootModelFromTileIndex(index);
            OnTileHovered?.Invoke(lootModel, index);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnCursorChangedHoverOverGrid?.Invoke(Model, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnCursorChangedHoverOverGrid?.Invoke(Model, false);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            foreach(var tileView in _gridTilesDict.Values)
            {
                tileView.OnTileHovered += InovkeGridTileHovered;
                tileView.OnTileClicked += InovkeTileClicked;
            }
        }

        private void DeregisterEvents()
        {
            foreach(var tileView in _gridTilesDict.Values)
            {
                tileView.OnTileHovered -= InovkeGridTileHovered;
                tileView.OnTileClicked -= InovkeTileClicked;
            }
        }

        #endregion
    }
}

