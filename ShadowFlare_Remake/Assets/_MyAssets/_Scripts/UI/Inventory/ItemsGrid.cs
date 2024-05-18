using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory {
    public class ItemsGrid : MonoBehaviour {

        public event Action<int,int> OnTileClicked;

        private float _tileWidth = 80;
        private float _tileHight = 80;

        private RectTransform _rectTransform;
        private Vector2 _mousePositionOnGrid = new Vector2();
        private Vector2Int _tileGridPosition = new Vector2Int();

        private InventoryItem[,] _inventoryItemSlot;

        [Header("Settings")]
        [SerializeField] private int gridSizeWidth = 10;
        [SerializeField] private int gridSizeHeight = 4;

        [Header("Debug")]
        [SerializeField] private GameObject _inventoryItemPrefab;

        private void Start() {

            _rectTransform = GetComponent<RectTransform>();

            //Init(gridSizeWidth, gridSizeHeight);
            //InventoryItem inventoryItem = Instantiate(_inventoryItemPrefab).GetComponent<InventoryItem>();
            //PlaceItem(inventoryItem, 3, 2);
        }

        private void OnEnable() {
            InitCellSize();
        }

        private void InitCellSize() {

            _tileWidth = 80;
            _tileHight = 80;

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            float widthRatioDiff = (float)screenWidth / 1920;
            float heightRatioDiff = (float)screenHeight / 1080;

            _tileWidth = _tileWidth * widthRatioDiff;
            _tileHight = _tileHight * heightRatioDiff;
        }

        private void Init(int width, int height) {

            _inventoryItemSlot = new InventoryItem[width, height];
            Vector2 size = new Vector2(width * _tileWidth, height * _tileHight);
            _rectTransform.sizeDelta = size;
        }

        public Vector2Int GetTileGridPosition(Vector2 mousePosition) {

            _mousePositionOnGrid.x = mousePosition.x - _rectTransform.position.x;
            _mousePositionOnGrid.y = _rectTransform.position.y - mousePosition.y;

            print("Mouse position on grid: " + _mousePositionOnGrid);

            _tileGridPosition.x = (int)(_mousePositionOnGrid.x / _tileWidth);
            _tileGridPosition.y = (int)(_mousePositionOnGrid.y / _tileHight);

            return _tileGridPosition;
        }

        public void PlaceItem(InventoryItem inventoryItem, int posX, int posY) {

            RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
            rectTransform.SetParent(_rectTransform);
            _inventoryItemSlot[posX, posY] = inventoryItem;

            Vector2 position = new Vector2();
            position.x = posX * gridSizeWidth + gridSizeWidth / 2;
            position.y = -(posY * gridSizeHeight + gridSizeHeight / 2);

            rectTransform.localPosition = position;
        }
    }
}

