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

        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        private Dictionary<Vector2Int, GridTileView> _gridTilesDict = new();

        protected override void Initialize()
        {
            CacheNulls();
            InitGridTilesDict();
        }

        protected override void ModelChanged()
        {
            PlaceItems();
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        private void CacheNulls()
        {
            if(_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
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

                //var tileView = _gridTilesDict[index].transform;
                //var itemRect = loot.transform;
                //itemRect.SetParent(tileView);
                //itemRect.localPosition = Vector3.zero;

                //var gridTileView = _gridTilesDict[index];
                //gridTileView.SetModel(gridTileModel);
            }
        }

        private void InovkeTileClicked(Vector2Int index)
        {
            var lootModel = Model.HeldLootDict[index];
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

        //private Vector2 _mousePositionOnGrid = new Vector2();
        //private Vector2Int _tileGridPosition = new Vector2Int();

        //public void TileClicked() {

        //    var clickedTile = GetTileGridPosition(Mouse.current.position.ReadValue());
        //    OnTileClicked?.Invoke(clickedTile);
        //}

        //private Vector2Int GetTileGridPosition(Vector2 mousePosition) {

        //    _mousePositionOnGrid.x = mousePosition.x - _rectTransform.position.x;
        //    _mousePositionOnGrid.y = _rectTransform.position.y - mousePosition.y;

        //    _tileGridPosition.x = (int)(_mousePositionOnGrid.x / Model.TileWidth);
        //    _tileGridPosition.y = (int)(_mousePositionOnGrid.y / Model.TileHight);

        //    return _tileGridPosition;
        //}


        //public void PlaceItem(InventoryItem inventoryItem, int posX, int posY) {

        //    RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        //    rectTransform.SetParent(_rectTransform);
        //    //_inventoryItemSlot[posX, posY] = inventoryItem;

        //    Vector2 position = new Vector2();
        //    //position.x = posX * gridSizeWidth + gridSizeWidth / 2;
        //    //position.y = -(posY * gridSizeHeight + gridSizeHeight / 2);

        //    position.x = posX;
        //    position.y = posY;

        //    rectTransform.localPosition = position;
        //}
    }
}

