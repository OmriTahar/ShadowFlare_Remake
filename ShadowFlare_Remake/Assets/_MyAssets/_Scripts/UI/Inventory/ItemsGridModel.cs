using ShadowFlareRemake.Loot;
using ShadowFlareRemake.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI
{
    public class ItemsGridModel : Model
    {
        public Dictionary<Vector2Int, GridTileModel> GridTileModelsDict { get; private set; } = new();
        public Dictionary<Vector2Int, LootModel> HeldLootDict { get; private set; } = new();

        public string Name { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        //public int TileWidth { get; private set; }
        //public int TileHight { get; private set; }

        public ItemsGridModel(string name, int gridWidth, int gridHeight /*,int tileWidth, int tileHight*/)
        {
            Name = name;
            //InitTileSize(tileWidth, tileHight);
            InitIGridTilesAndLootDicts(gridWidth,gridHeight);
        }

        public void PlaceLootOnGrid(Vector2Int tileIndex, LootModel lootModel)
        {
            //HeldLootDict[tileIndex] = lootModel;
            GridTileModelsDict[tileIndex].SetLootModel(lootModel);
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
            HeldLootDict[tileIndex] = null;
        }

        private bool IsValidPlacement(LootView inventoryItem, Vector2Int tileIndex)
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

        //private void InitTileSize(float tileWidth, float tileHight)
        //{
        //    var screenWidth = Screen.width;
        //    var screenHeight = Screen.height;

        //    float widthRatioDiff = (float)screenWidth / 1920;
        //    float heightRatioDiff = (float)screenHeight / 1080;

        //    TileWidth = (int)(tileWidth * widthRatioDiff);
        //    TileHight = (int)(tileHight * heightRatioDiff);
        //}

        private void InitIGridTilesAndLootDicts(int gridWidth, int gridHight)
        {
            GridWidth = gridWidth;
            GridHeight = gridHight;

            for(int x = 0; x < gridWidth; x++)
            {
                for(int y = 0; y < gridHight; y++)
                {
                    var tileIndex = new Vector2Int(x, y);
                    GridTileModelsDict.Add(tileIndex, new GridTileModel(tileIndex));
                    HeldLootDict.Add(tileIndex, null);
                }
            }
        }
    }
}
