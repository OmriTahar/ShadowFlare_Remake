using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserModel : Model
    {
        public LootModel CurentHoveredLootModel { get; private set; }
        public Vector2Int CurrentHoveredLootModelRootIndex { get; private set; }
        public LootModel CurrentHeldLootModel { get; private set; }
        public CursorIconState CurrentCursorIconState { get; private set; }

        #region Initialization

        public CurserModel(CursorIconState cursorState = CursorIconState.Move)
        {
            SetCursorIconState(cursorState);
        }

        #endregion

        public void SetCursorIconState(CursorIconState newCursorState)
        {
            if(newCursorState == CurrentCursorIconState)
                return;

            CurrentCursorIconState = newCursorState;
            Changed();
        }

        public bool IsHoldingLoot()
        {
            return CurrentHeldLootModel != null;
        }

        public void PickUpLoot(LootModel lootModel)
        {
            CurrentHeldLootModel = lootModel;
            Changed();
        }

        public void DropLoot()
        {
            CurrentHeldLootModel = null;
            Changed();
        }

        public void SetCurrentHoveredLootModel(LootModel lootModel, Vector2Int rootIndex)
        {
            CurentHoveredLootModel = lootModel;
            CurrentHoveredLootModelRootIndex = rootIndex;
        }

        //public ItemsGridModel CurrentHoveredItemsGridModel { get; private set; }

        //public void SetCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel)
        //{
        //    CurrentHoveredItemsGridModel = itemsGridModel;
        //}


        //public void PlaceLootInGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        //{
        //    var tuple = itemsGridModel.TryHandPlaceLootOnGrid(tileIndex, lootModel);
        //    var isLootPlaced = tuple.Item1;
        //    var swappedLoot = tuple.Item2;

        //    if(!isLootPlaced)
        //        return;

        //    if(swappedLoot != null)
        //    {
        //        PickUpLootLogic(swappedLoot);
        //    }
        //    else
        //    {
        //        DropLootLogic();
        //    }

        //    Changed();
        //}

    }
}

