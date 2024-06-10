using ShadowFlareRemake.Loot;
using ShadowFlareRemake.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI
{
    public class ItemsGridModel : Model
    {
        public Dictionary<Vector2Int, GridTileModel> GridTileModelsDict { get; private set; } = new();

        public string Name { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }

        //public int TileWidth { get; private set; }
        //public int TileHight { get; private set; }

        private Vector2Int _topLeftValidIndex;

        public ItemsGridModel(string name, int gridWidth, int gridHeight /*,int tileWidth, int tileHight*/)
        {
            Name = name;
            //InitTileSize(tileWidth, tileHight);
            InitIGridTilesDict(gridWidth, gridHeight);
        }

        public (bool, LootModel) TryPlaceLootOnGrid(Vector2Int tileIndex, LootModel lootModel)
        {
            GridTileModelsDict.TryGetValue(tileIndex, out GridTileModel gridTileModel);
            var swappedLoot = gridTileModel.LootModel;
            RemoveItemFromGrid(tileIndex, false);

            if(!IsValidPlacement(lootModel.LootData.Width, lootModel.LootData.Height, tileIndex))
            {
                GridTileModelsDict[tileIndex].SetLootModel(swappedLoot);
                return (false, lootModel);
            }

            GridTileModelsDict[_topLeftValidIndex].SetLootModel(lootModel);
            Changed();

            SetTopLeftValidIndex(-1, -1);
            return (true, swappedLoot);
        }

        public void RemoveItemFromGrid(Vector2Int tileIndex, bool invokeChanged)
        {
            GridTileModelsDict[tileIndex].SetLootModel(null);

            if(invokeChanged)
            {
                Changed();
            }
        }

        private bool IsValidPlacement(int width, int height, Vector2Int tileIndex)
        {
            var isValidHorizontally = IsValidPlacementHorizontally(width, tileIndex);
            var isValidVertically = IsValidPlacementVertically(height, tileIndex);

            var isValidPlacement = isValidHorizontally && isValidVertically;
            Debug.Log($"Inventory: Is Valid Loot Placement: {isValidPlacement}");

            return isValidPlacement;
        }

        private bool IsValidPlacementHorizontally(int width, Vector2Int tileIndex)
        {
            SetTopLeftValidIndex(tileIndex.x, tileIndex.y);

            var indexCheck = new Vector2Int();
            var consecutiveTilesCounter = 0;
            var helper = 0;

            indexCheck.y = tileIndex.y;

            for(int i = -width + 1; i < width; i++)
            {
                indexCheck.x = tileIndex.x + i;

                if(!IsValidTileIndex(indexCheck))
                {
                    continue;
                }

                GridTileModelsDict.TryGetValue(indexCheck, out GridTileModel gridTileModel);

                if(gridTileModel.LootModel == null)
                {
                    if(indexCheck.x < _topLeftValidIndex.x)
                        SetTopLeftValidIndex(tileIndex.x, _topLeftValidIndex.y);

                    helper = 1;
                }
                else
                    helper = -1;

                consecutiveTilesCounter += helper;
            }

            if(consecutiveTilesCounter >= width)
                return true;

            SetTopLeftValidIndex(-1, -1);
            return false;
        }

        private bool IsValidPlacementVertically(int height, Vector2Int tileIndex)
        {
            SetTopLeftValidIndex(tileIndex.x, tileIndex.y);

            var indexCheck = new Vector2Int();
            var consecutiveTilesCounter = 0;
            var helper = 0;

            indexCheck.x = tileIndex.x;

            for(int i = -height + 1; i < height; i++)
            {
                indexCheck.y = tileIndex.y + i;

                if(!IsValidTileIndex(indexCheck))
                {
                    continue;
                }

                GridTileModelsDict.TryGetValue(indexCheck, out GridTileModel gridTileModel);

                if(gridTileModel.LootModel == null)
                {
                    if(indexCheck.y < _topLeftValidIndex.y)
                        SetTopLeftValidIndex(_topLeftValidIndex.x, indexCheck.y);

                    helper = 1;
                }
                else
                    helper = -1;

                consecutiveTilesCounter += helper;
            }

            if(consecutiveTilesCounter >= height)
                return true;

            SetTopLeftValidIndex(-1, -1);
            return false;
        }

        private bool IsValidPlacementVertically2(int height, Vector2Int tileIndex)
        {
            SetTopLeftValidIndex(tileIndex.x, tileIndex.y);

            var indexCheck = new Vector2Int();
            var consecutiveTilesCounter = 0;
            var helper = 0;

            indexCheck.x = tileIndex.x;

            for(int i = -height + 1; i < height; i++)
            {
                indexCheck.y = tileIndex.y + i;

                if(!IsValidTileIndex(indexCheck))
                {
                    continue;
                }

                GridTileModelsDict.TryGetValue(indexCheck, out GridTileModel gridTileModel);

                if(gridTileModel.LootModel == null)
                {
                    if(indexCheck.y < _topLeftValidIndex.y)
                        SetTopLeftValidIndex(_topLeftValidIndex.x, indexCheck.y);

                    helper = 1;
                }
                else
                    helper = -1;

                consecutiveTilesCounter += helper;
            }

            if(consecutiveTilesCounter >= height)
                return true;

            SetTopLeftValidIndex(-1, -1);
            return false;
        }

        private bool IsValidTileIndex(Vector2Int indexCheck)
        {
            if(indexCheck.x < 0 || indexCheck.x > GridWidth - 1 || indexCheck.y < 0 || indexCheck.y > GridHeight - 1)
            {
                return false;
            }

            return true;
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

        private void InitIGridTilesDict(int gridWidth, int gridHight)
        {
            GridWidth = gridWidth;
            GridHeight = gridHight;

            for(int x = 0; x < gridWidth; x++)
            {
                for(int y = 0; y < gridHight; y++)
                {
                    var tileIndex = new Vector2Int(x, y);
                    GridTileModelsDict.Add(tileIndex, new GridTileModel(tileIndex));
                }
            }
        }

        private void SetTopLeftValidIndex(int x, int y)
        {
            _topLeftValidIndex.x = x;
            _topLeftValidIndex.y = y;
        }
    }
}
