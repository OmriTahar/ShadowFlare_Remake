using ShadowFlareRemake.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI
{
    public class ItemsGridModel : Model
    {
        public Dictionary<Vector2Int, Item> ItemsDict { get; private set; } = new();

        public string Name { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHight { get; private set; }

        public ItemsGridModel(string name, int tileWidth, int tileHight)
        {
            Name = name;
            HandleInitTiles(tileWidth, tileHight);
        }

        public void PlaceItemOnGrid(Vector2Int tileIndex, Item item)
        {
            ItemsDict[tileIndex] = item;
            Changed();

            //if(!IsValidPlacement(PickedItem, tileIndex))
            //{
            //    return;
            //}


            //var isIndexTaken = ItemsDict.TryGetValue(tileIndex, out var carriedItem);

            //if(!isIndexTaken) {
            //    ItemsDict.Add(tileIndex, item);

            //} else {
            //    ItemsDict[tileIndex] = item;
            //}
        }

        public void RemoveItemFromGrid(Vector2Int tileIndex)
        {
            ItemsDict[tileIndex] = null;
        }

        private bool IsValidPlacement(Item inventoryItem, Vector2Int tileIndex)
        {
            bool isValid;

            if(inventoryItem == null)
            {
                isValid = false;
            }
            else
            {
                isValid = true;

            }

            Debug.Log($"Inventory: Is Valid Placement: {isValid}");
            return isValid;
        }

        private void HandleInitTiles(float tileWidth, float tileHight)
        {
            InitTileSize(tileWidth, tileHight);
            InitItemsDict();
        }

        private void InitTileSize(float tileWidth, float tileHight)
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            float widthRatioDiff = (float)screenWidth / 1920;
            float heightRatioDiff = (float)screenHeight / 1080;

            TileWidth = (int)(tileWidth * widthRatioDiff);
            TileHight = (int)(tileHight * heightRatioDiff);
        }

        private void InitItemsDict()
        {
            for(int x = 0; x < TileWidth; x++)
            {
                for(int y = 0; y < TileWidth; y++)
                {
                    ItemsDict.Add(new Vector2Int(x, y), null);
                }
            }
        }
    }
}
