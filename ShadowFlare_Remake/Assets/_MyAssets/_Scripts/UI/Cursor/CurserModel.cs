using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserModel : Model
    {
        public ItemsGridType CurrentHoveredItemsGridType { get; private set; } = ItemsGridType.None;  
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

        #region Meat & Potatos

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
            if(lootModel == CurentHoveredLootModel)
                return;

            CurentHoveredLootModel = lootModel;
            CurrentHoveredLootModelRootIndex = rootIndex;
            Changed();
        }

        public void SetCurrentHoveredItemsGridType(ItemsGridType itemsGridType)
        {
            CurrentHoveredItemsGridType = itemsGridType;
            Changed();
        }

        #endregion
    }
}

