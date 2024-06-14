using ShadowFlareRemake.Enums;
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
        public bool IsSingleTile { get; private set; }
        public bool HasPlaceHolderSprite { get; private set; }

        private Dictionary<Vector2Int, Vector2Int> _heldLootRootIndexesDict = new();
        private Vector2Int _topLeftValidIndex;
        private LootType _acceptedLootType;

        private readonly Vector2Int _singleTileIndex = Vector2Int.zero;
        private readonly Vector2Int _emptyTileIndex = new Vector2Int(-1, -1);

        #region Init

        public ItemsGridModel(string name, int gridWidth, int gridHeight, LootType acceptedLootType, bool hasPlaceHolderSprite = true)
        {
            Name = name;
            HasPlaceHolderSprite = hasPlaceHolderSprite;
            IsSingleTile = (gridWidth == 1 && gridHeight == 1);
            _acceptedLootType = acceptedLootType;
            InitGridTilesModelDictAndHeldItemsIndexesDict(gridWidth, gridHeight);
        }

        private void InitGridTilesModelDictAndHeldItemsIndexesDict(int gridWidth, int gridHight)
        {
            if(IsSingleTile)
            {
                var tileIndex = _singleTileIndex;
                GridTileModelsDict.Add(tileIndex, new GridTileModel(tileIndex, true));
                _heldLootRootIndexesDict.Add(tileIndex, _emptyTileIndex);
                return;
            }

            GridWidth = gridWidth;
            GridHeight = gridHight;

            for(int x = 0; x < gridWidth; x++)
            {
                for(int y = 0; y < gridHight; y++)
                {
                    var tileIndex = new Vector2Int(x, y);
                    GridTileModelsDict.Add(tileIndex, new GridTileModel(tileIndex, false));
                    _heldLootRootIndexesDict.Add(tileIndex, _emptyTileIndex);
                }
            }
        }

        #endregion

        #region NEW - Place & Remove 

        public bool TryAutoPlaceLootOnGrid(LootModel lootModel)
        {
            bool isAccepetedLootType = _acceptedLootType == LootType.All || lootModel.LootData.Type == _acceptedLootType;

            if(!isAccepetedLootType || lootModel == null)
            {
                return false;
            }

            bool isSuccess;

            foreach(var tileIndex in _heldLootRootIndexesDict.Keys)
            {
                isSuccess = TryAutoPlaceLogic(tileIndex, lootModel);

                if(isSuccess)
                    return true;
            }

            return false;
        }

        private bool TryAutoPlaceLogic(Vector2Int tileIndex, LootModel lootModel)
        {
            if(IsSingleTile)
            {
                SetTopLeftValidIndex(_singleTileIndex);
            }

            if(!IsValidPlacement(lootModel.LootData.Width, lootModel.LootData.Height, tileIndex))
            {
                return false;
            }

            PlaceItemOnGrid(lootModel, true);
            SetTopLeftValidIndex(_emptyTileIndex);
            return true;
        }

        public (bool, LootModel) TryHandPlaceLootOnGrid(Vector2Int tileIndex, LootModel lootModel, bool validateLootType = true)
        {
            if(validateLootType)
            {
                bool isAccepetedLootType = _acceptedLootType == LootType.All || lootModel.LootData.Type == _acceptedLootType;

                if(!isAccepetedLootType || lootModel == null)
                {
                    return (false, lootModel);
                }
            }

            _heldLootRootIndexesDict.TryGetValue(tileIndex, out Vector2Int rootLootIndex);
            LootModel swappedLoot = null;

            if(rootLootIndex.x != -1)
            {
                GridTileModelsDict.TryGetValue(rootLootIndex, out GridTileModel gridTileModel);
                swappedLoot = gridTileModel.LootModel;
                RemoveItemFromGrid(rootLootIndex, false);
            }

            if(IsSingleTile)
            {
                SetTopLeftValidIndex(_singleTileIndex);
            }
            else if(!IsSingleTile && !IsValidPlacement(lootModel.LootData.Width, lootModel.LootData.Height, tileIndex))
            {
                if(swappedLoot != null)
                {
                    SetTopLeftValidIndex(rootLootIndex);
                    PlaceItemOnGrid(swappedLoot, false);
                }
                return (false, lootModel);
            }

            PlaceItemOnGrid(lootModel, true);
            SetTopLeftValidIndex(_emptyTileIndex);
            return (true, swappedLoot);
        }

        private void PlaceItemOnGrid(LootModel lootModel, bool invokeChanged)
        {
            GridTileModelsDict[_topLeftValidIndex].SetLootModel(lootModel);

            var lootOtherIndexesHolder = new Vector2Int();

            for(int x = 0; x < lootModel.LootData.Width; x++)
            {
                lootOtherIndexesHolder.x = _topLeftValidIndex.x + x;

                for(int y = 0; y < lootModel.LootData.Height; y++)
                {
                    lootOtherIndexesHolder.y = _topLeftValidIndex.y + y;
                    _heldLootRootIndexesDict[lootOtherIndexesHolder] = _topLeftValidIndex;
                }
            }

            if(invokeChanged)
            {
                Changed();
            }
        }

        public void RemoveItemFromGrid(Vector2Int tileIndex, bool invokeChanged)
        {
            var rootIndex = _heldLootRootIndexesDict[tileIndex];
            GridTileModelsDict[rootIndex].SetLootModel(null);

            var indexesToRemove = new List<Vector2Int>();

            foreach(var keyValuePair in _heldLootRootIndexesDict)
            {
                var tileIndexCheck = keyValuePair.Key;
                var lootRootIndexCheck = keyValuePair.Value;

                if(lootRootIndexCheck == rootIndex)
                {
                    indexesToRemove.Add(tileIndexCheck);
                }
            }

            foreach(var index in indexesToRemove)
            {
                _heldLootRootIndexesDict[index] = _emptyTileIndex;
            }

            if(invokeChanged)
            {
                Changed();
            }
        }

        private bool IsValidPlacement(int width, int height, Vector2Int tileIndex)
        {
            SetTopLeftValidIndex(tileIndex.x, tileIndex.y);
            var isValidHorizontally = IsValidPlacementHorizontally(width, tileIndex);
            var isValidVertically = IsValidPlacementVertically(height, tileIndex);
            var isValidPlacement = isValidHorizontally && isValidVertically;

            Debug.Log($"Items Grid Model: Is Valid Loot Placement: {isValidPlacement}. (Horizontal: {isValidHorizontally} | (Vertical: {isValidVertically})");
            return isValidPlacement;
        }

        private bool IsValidPlacementHorizontally(int width, Vector2Int tileIndex)
        {
            var tileIndexCheck = new Vector2Int();
            var consecutiveTilesCounter = 0;

            tileIndexCheck.y = tileIndex.y;

            for(int i = -width + 1; i < width; i++)
            {
                tileIndexCheck.x = tileIndex.x + i;

                if(!IsValidTileIndex(tileIndexCheck))
                    continue;

                _heldLootRootIndexesDict.TryGetValue(tileIndexCheck, out Vector2Int lootIRootndex);

                if(lootIRootndex.x == -1)
                {
                    if(tileIndexCheck.x < _topLeftValidIndex.x)
                        SetTopLeftValidIndex(tileIndexCheck.x, _topLeftValidIndex.y);

                    consecutiveTilesCounter++;

                    if(consecutiveTilesCounter >= width)
                        return true;
                }
                else
                    consecutiveTilesCounter = 0;
            }

            SetTopLeftValidIndex(_emptyTileIndex);
            return false;
        }

        private bool IsValidPlacementVertically(int height, Vector2Int tileIndex)
        {
            var tileIndexCheck = new Vector2Int();
            var consecutiveTilesCounter = 0;

            tileIndexCheck.x = tileIndex.x;

            for(int i = -height + 1; i < height; i++)
            {
                tileIndexCheck.y = tileIndex.y + i;

                if(!IsValidTileIndex(tileIndexCheck))
                {
                    continue;
                }

                _heldLootRootIndexesDict.TryGetValue(tileIndexCheck, out Vector2Int lootIRootndex);

                if(lootIRootndex.x == -1)
                {
                    if(tileIndexCheck.y < _topLeftValidIndex.y)
                        SetTopLeftValidIndex(_topLeftValidIndex.x, tileIndexCheck.y);

                    consecutiveTilesCounter++;

                    if(consecutiveTilesCounter >= height)
                        return true;
                }
                else
                    consecutiveTilesCounter = 0;
            }

            SetTopLeftValidIndex(_emptyTileIndex);
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

        private void SetTopLeftValidIndex(int x, int y)
        {
            _topLeftValidIndex.x = x;
            _topLeftValidIndex.y = y;
        }

        private void SetTopLeftValidIndex(Vector2Int tileIndex)
        {
            _topLeftValidIndex = tileIndex;
        }

        #endregion

        #region Helpers

        public LootModel GetLootModelFromTileIndex(Vector2Int tileIndex)
        {
            if(_heldLootRootIndexesDict[tileIndex].x != -1)
            {
                var rootIndex = _heldLootRootIndexesDict[tileIndex];
                return GridTileModelsDict[rootIndex].LootModel;
            }

            return null;
        }

        public bool IsGridContainsLoot()
        {
            foreach(var value in GridTileModelsDict.Values)
            {
                if(value.LootModel != null)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
