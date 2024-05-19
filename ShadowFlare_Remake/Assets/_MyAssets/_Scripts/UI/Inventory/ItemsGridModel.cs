using ShadowFlareRemake.UI.Inventory;
using UnityEngine;

namespace ShadowFlareRemake.UI {
    public class ItemsGridModel : Model {

        public float TileWidth { get; private set; }
        public float TileHight { get; private set; }

        public ItemsGridModel(float tileWidth, float tileHight) {

            InitCellSize(tileWidth, tileHight);
        }

        public void PlaceItem(InventoryItem inventoryItem, int posX, int posY) {

        }

        private void InitCellSize(float tileWidth, float tileHight) {

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            float widthRatioDiff = (float)screenWidth / 1920;
            float heightRatioDiff = (float)screenHeight / 1080;

            TileWidth = tileWidth * widthRatioDiff;
            TileHight = tileHight * heightRatioDiff;
        }
    }
}
