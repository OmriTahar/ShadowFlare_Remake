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
        public ItemsGridType ItemsGridType { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public bool IsSingleTile { get; private set; }
        public bool HasPlaceHolderSprite { get; private set; }

        private Dictionary<Vector2Int, Vector2Int> _heldLootRootIndexesDict = new();
        private Vector2Int _topLeftValidIndex;
        private List<LootType> _acceptedLootTypes;

        private readonly Vector2Int _singleTileIndex = Vector2Int.zero;
        private readonly Vector2Int _emptyTileIndex = new Vector2Int(-1, -1);

        #region Initialization

        public ItemsGridModel(ItemsGridType itemsGridType, int gridWidth, int gridHeight, List<LootType> acceptedLootTypes, bool hasPlaceHolderSprite = true)
        {
            ItemsGridType = itemsGridType;
            HasPlaceHolderSprite = hasPlaceHolderSprite;
            IsSingleTile = (gridWidth == 1 && gridHeight == 1);
            _acceptedLootTypes = acceptedLootTypes;
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

        #region Place & Remove 

        public bool TryAutoPlaceLootOnGrid(LootModel lootModel)
        {
            if(lootModel == null || !IsAccepetedLootType(lootModel.LootData.LootType))
                return false;

            bool isSuccess;

            foreach(var tileIndex in _heldLootRootIndexesDict.Keys)
            {
                isSuccess = TryAutoPlaceLogic(tileIndex, lootModel);

                if(isSuccess)
                    return true;
            }

            return false;
        }

        public bool TryAutoPlaceGoldOnGrid(LootModel lootModel)
        {
            var goldLootModels = GetHeldGoldLootModels();
            var spareGold = 0;

            if(goldLootModels.Count > 0)
            {
                var amountToAdd = lootModel.GoldAmount;

                foreach(var placedModel in goldLootModels)
                {
                    var newAmount = placedModel.GoldAmount + amountToAdd;
                    spareGold = placedModel.SetGoldAmountAndGetSpare(newAmount, false);

                    if(spareGold > 0)
                    {
                        amountToAdd = spareGold;
                        continue;
                    }

                }

                if(spareGold == 0)
                {
                    return true;
                }

                lootModel.SetGoldAmountAndGetSpare(spareGold, false);
            }

            return TryAutoPlaceLootOnGrid(lootModel);
        }

        //public int CombineGoldData(GoldData_ScriptableObject other)
        //{
        //    int spareGold = 0;

        //    if(Amount + other.Amount <= MaxGoldAmount)
        //    {
        //        Amount += other.Amount;
        //    }
        //    else
        //    {
        //        var total = Amount + other.Amount;
        //        Amount = MaxGoldAmount;
        //        spareGold = total - Amount;
        //    }

        //    return spareGold;
        //}

        private bool IsAccepetedLootType(LootType lootType)
        {
            foreach(var acceptedLootType in _acceptedLootTypes)
            {
                if(acceptedLootType == LootType.All || acceptedLootType == lootType)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryAutoPlaceLogic(Vector2Int tileIndex, LootModel lootModel)
        {
            if(IsSingleTile)
            {
                if(_heldLootRootIndexesDict[_singleTileIndex].x != -1)
                    return false;

                SetTopLeftValidIndex(_singleTileIndex);
            }
            else if(!IsSingleTile && !IsValidPlacement(lootModel.LootData.Width, lootModel.LootData.Height, tileIndex))
            {
                return false;
            }

            PlaceItemOnGrid(lootModel, true);
            SetTopLeftValidIndex(_emptyTileIndex);
            return true;
        }

        public (bool, LootModel) TryHandPlaceLootOnGrid(Vector2Int tileIndex, LootModel lootModel)
        {
            if(lootModel == null || !IsAccepetedLootType(lootModel.LootData.LootType))
                return (false, lootModel);

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

        public LootModel RemoveItemFromGrid(Vector2Int tileIndex, bool invokeChanged)
        {
            var rootIndex = _heldLootRootIndexesDict[tileIndex];
            var removedLootModel = GridTileModelsDict[rootIndex].LootModel;
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

            return removedLootModel;
        }

        private bool IsValidPlacement(int width, int height, Vector2Int tileIndex)
        {
            SetTopLeftValidIndex(tileIndex.x, tileIndex.y);
            var isValidHorizontally = IsValidPlacementHorizontally(width, tileIndex);
            var isValidVertically = IsValidPlacementVertically(height, tileIndex);
            return isValidHorizontally && isValidVertically;
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

        public Vector2Int GetLootModelRootIndexByType(LootType lootType)
        {
            foreach(var keyValuePair in GridTileModelsDict)
            {
                var rootIndex = keyValuePair.Key;
                var gridTileModel = keyValuePair.Value;

                if(gridTileModel.LootModel == null)
                    continue;

                if(gridTileModel.LootModel.LootData.LootType == lootType)
                {
                    return rootIndex;
                }
            }

            return _emptyTileIndex;
        }

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

        public List<LootModel> GetHeldGoldLootModels()
        {
            if(ItemsGridType != ItemsGridType.Carry)
                return null;

            var goldLootModels = new List<LootModel>();

            foreach(var gridTileModel in GridTileModelsDict.Values)
            {
                var lootModel = gridTileModel.LootModel;

                if(lootModel == null)
                    continue;

                if(lootModel.LootCategory == LootCategory.Gold)
                {
                    goldLootModels.Add(lootModel);
                }
            }

            return goldLootModels;
        }

        #endregion
    }
}
