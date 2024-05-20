using ShadowFlareRemake.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI {
    public class ItemsGridModel : Model {

        public Dictionary<Vector2Int, InventoryItem> ItemsDict { get; private set; } = new();
        public string Name { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHight { get; private set; }

        public ItemsGridModel(string name, int tileWidth, int tileHight) {

            Name = name;
            InitCellSize(tileWidth, tileHight);
        }

        public void PlaceItem(InventoryItem item, Vector2Int tileIndex) {

            if(!IsValidPlacement(item, tileIndex)) {
                return;
            }

            var isIndexTaken = ItemsDict.TryGetValue(tileIndex, out var carriedItem);

            if(!isIndexTaken) {
                ItemsDict.Add(tileIndex, item);
            }

            Changed();
        }

        private bool IsValidPlacement(InventoryItem inventoryItem, Vector2Int tileIndex) {

            if(inventoryItem == null) {
                return false;
            }

            return true;
        }

        private void InitCellSize(float tileWidth, float tileHight) {

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            float widthRatioDiff = (float)screenWidth / 1920;
            float heightRatioDiff = (float)screenHeight / 1080;

            TileWidth = (int)(tileWidth * widthRatioDiff);
            TileHight = (int)(tileHight * heightRatioDiff);
        }
    }
}
