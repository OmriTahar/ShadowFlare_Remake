using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI.Inventory {
    public class ItemsGridView : View<ItemsGridModel>, IPointerEnterHandler, IPointerExitHandler {

        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;
        public event Action<Vector2Int> OnTileClicked;
        public event Action OnItemsPlaced;

        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        private Dictionary<Vector2Int, GridTileView> _gridTilesDict = new();

        private Vector2 _mousePositionOnGrid = new Vector2();
        private Vector2Int _tileGridPosition = new Vector2Int();

        protected override void Initialize() {

            CacheNulls();
            InitGridTilesDict();
        }

        protected override void ModelChanged() {

            PlaceItems();
        }

        protected override void Clean() {

            DeregisterEvents();
        }

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

        private void PlaceItems() {

            foreach(var item in Model.ItemsDict) {

                if(item.Value == null) {
                    continue;
                }

                var viewTransform = _gridTilesDict[item.Key].transform;
                var rect = item.Value.transform;
                rect.SetParent(viewTransform);
                rect.localPosition = Vector3.zero;
            }

            OnItemsPlaced?.Invoke();
        }

        public void PlaceItem(InventoryItem inventoryItem, int posX, int posY) {

            RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
            rectTransform.SetParent(_rectTransform);
            //_inventoryItemSlot[posX, posY] = inventoryItem;

            Vector2 position = new Vector2();
            //position.x = posX * gridSizeWidth + gridSizeWidth / 2;
            //position.y = -(posY * gridSizeHeight + gridSizeHeight / 2);

            position.x = posX;
            position.y = posY;

            rectTransform.localPosition = position;
        }

        private void CacheNulls() {

            if(_rectTransform == null) {
                _rectTransform = GetComponent<RectTransform>();
            }
        }

        private void InitGridTilesDict() {

            var tileViews = GetComponentsInChildren<GridTileView>();

            foreach (var view in tileViews) {

                _gridTilesDict.Add(view.Index, view);
                view.OnTileClicked += InovkeTileClicked;
            }
        }

        private void InovkeTileClicked(Vector2Int index) {

             OnTileClicked?.Invoke(index);
        }

        public void OnPointerEnter(PointerEventData eventData) {

            OnCursorChangedHoverOverGrid?.Invoke(Model, true);
        }

        public void OnPointerExit(PointerEventData eventData) {

            OnCursorChangedHoverOverGrid?.Invoke(Model, false);
        }

        private void DeregisterEvents() {

            foreach(var tileView in _gridTilesDict.Values) {

                tileView.OnTileClicked -= InovkeTileClicked;
            }
        }
    }
}

