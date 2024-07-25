using ShadowFlareRemake.Loot;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI.ItemsGrid
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

        public bool TryAutoPlace_Loot(LootModel lootModel)
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

        public bool TryAutoPlace_Gold(LootModel lootModel)
        {
            var goldLootModels = GetHeldGoldLootModels();
            var spareGold = 0;

            if(goldLootModels.Count > 0)
            {
                var amountToAdd = lootModel.Amount;

                foreach(var placedModel in goldLootModels)
                {
                    var newAmount = placedModel.Amount + amountToAdd;
                    spareGold = placedModel.SetAmountAndGetSpareWhenMaxed(newAmount);

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

                lootModel.SetAmountAndGetSpareWhenMaxed(spareGold);
            }

            return TryAutoPlace_Loot(lootModel);
        }

        public (bool, LootModel) TryHandPlace_Loot(Vector2Int tileIndex, LootModel lootModel)
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

        public (bool, LootModel) TryHandPlace_Gold(Vector2Int tileIndex, LootModel lootModel)
        {
            if(lootModel == null || !IsAccepetedLootType(lootModel.LootData.LootType))
                return (false, lootModel);

            _heldLootRootIndexesDict.TryGetValue(tileIndex, out Vector2Int rootLootIndex);
            LootModel swappedLoot = null;
            int spareGold = 0;
            bool isPlacedGold = false;

            if(rootLootIndex.x != -1)
            {
                GridTileModelsDict.TryGetValue(rootLootIndex, out GridTileModel gridTileModel);
                var placedLootModel = gridTileModel.LootModel;

                if(placedLootModel != null && placedLootModel.LootCategory != LootCategory.Gold)
                {
                    swappedLoot = placedLootModel;
                    RemoveItemFromGrid(rootLootIndex, false);
                }
                else
                {
                    var newAmount = placedLootModel.Amount + lootModel.Amount;
                    spareGold = placedLootModel.SetAmountAndGetSpareWhenMaxed(newAmount);
                    isPlacedGold = true;
                }
            }

            if(isPlacedGold)
            {
                SetTopLeftValidIndex(_emptyTileIndex);

                if(spareGold > 0)
                {
                    lootModel.SetAmountAndGetSpareWhenMaxed(spareGold);
                    return (false, lootModel);
                }

                return (true, null);
            }

            if(!IsValidPlacement(lootModel.LootData.Width, lootModel.LootData.Height, tileIndex))
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

        private bool IsValidPlacement(int lootWidth, int lootHeight, Vector2Int clickedTileIndex)
        {
            var totalTilesToCheck = GetTotalTilesForPlacementValidation(lootWidth, lootHeight, clickedTileIndex);
            var consecutiveTilesGoal = GetConsecutiveTilesGoalForPlacementValidation(lootWidth, lootHeight);
            var tileIndexCheck = new Vector2Int();
            var consecutiveTilesCounter = 0;

            foreach(var tile in totalTilesToCheck)
            {
                SetTopLeftValidIndex(tile);
                tileIndexCheck = tile;
                consecutiveTilesCounter = 0;

                if(!IsValidTileIndex(tileIndexCheck))
                {
                    continue;
                }

                var x = tileIndexCheck.x;
                var y = tileIndexCheck.y;
                var finalX = x + lootWidth;
                var finalY = y + lootHeight;

                for(int i = x; i < finalX; i++)
                {
                    tileIndexCheck.x = i;

                    for(int j = y; j < finalY; j++)
                    {
                        tileIndexCheck.y = j;

                        if(!IsValidTileIndex(tileIndexCheck))
                        {
                            continue;
                        }

                        _heldLootRootIndexesDict.TryGetValue(tileIndexCheck, out Vector2Int lootIRootndex);

                        if(lootIRootndex == _emptyTileIndex)
                        {
                            if(tileIndexCheck.x < _topLeftValidIndex.x)
                            {
                                SetTopLeftValidIndex(tileIndexCheck.x, _topLeftValidIndex.y);
                            }
                            if(tileIndexCheck.y < _topLeftValidIndex.y)
                            {
                                SetTopLeftValidIndex(_topLeftValidIndex.x, tileIndexCheck.y);
                            }

                            consecutiveTilesCounter++;

                            if(consecutiveTilesCounter >= consecutiveTilesGoal)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            consecutiveTilesCounter = 0;
                        }
                    }
                }
            }

            SetTopLeftValidIndex(_emptyTileIndex);
            return false;
        }

        private List<Vector2Int> GetTotalTilesForPlacementValidation(int lootWidth, int lootHeight, Vector2Int clickedTileIndex)
        {
            var totalTilesToCheck = new List<Vector2Int>();
            var tileIndexCheck = new Vector2Int();

            if(lootWidth == 1 && lootHeight == 1)
            {
                totalTilesToCheck.Add(clickedTileIndex);
            }
            else
            {
                for(int i = -lootWidth + 1; i < 1; i++)
                {
                    tileIndexCheck.x = clickedTileIndex.x + i;

                    for(int j = -lootHeight + 1; j < 1; j++)
                    {
                        tileIndexCheck.y = clickedTileIndex.y + j;
                        totalTilesToCheck.Add(tileIndexCheck);
                    }
                }
            }

            return totalTilesToCheck;
        }

        private int GetConsecutiveTilesGoalForPlacementValidation(int lootWidth, int lootHeight)
        {
            if(lootWidth == 1 && lootHeight == 1)
            {
                return 1;
            }

            if(lootWidth > 1 && lootHeight == 1)
            {
                return lootWidth;
            }

            if(lootWidth == 1 && lootHeight > 1)
            {
                return lootHeight;
            }

            if(lootWidth > 1 && lootHeight > 1)
            {
                return lootWidth * lootHeight;
            }

            return 100;
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
