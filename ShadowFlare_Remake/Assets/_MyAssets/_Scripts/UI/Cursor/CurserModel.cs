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
        public LootInfoTooltipModel LootInfoTooltipModel { get; private set; }

        #region Initialization

        public CurserModel(CursorIconState cursorState = CursorIconState.Move)
        {
            SetCursorIconState(cursorState);
            LootInfoTooltipModel = new LootInfoTooltipModel();
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
            lootModel.SetIsAllowedToSetLocalPosition(false);
            CurrentHeldLootModel = lootModel;
            Changed();
        }

        public void DropLoot()
        {
            CurrentHeldLootModel.SetIsAllowedToSetLocalPosition(true);
            CurrentHeldLootModel = null;
            Changed();
        }

        public void SetCurrentHoveredLootModel(LootModel lootModel, Vector2Int rootIndex)
        {
            if(lootModel == CurentHoveredLootModel)
                return;

            CurentHoveredLootModel = lootModel;
            CurrentHoveredLootModelRootIndex = rootIndex;
            HandleLootInfoTooltip();
            Changed();
        }

        public void SetCurrentHoveredItemsGridType(ItemsGridType itemsGridType)
        {
            CurrentHoveredItemsGridType = itemsGridType;
            HandleLootInfoTooltip();
            Changed();
        }

        public void DeactivateInfoTooltip()
        {
            LootInfoTooltipModel.SetIsActive(false);
        }

        private void HandleLootInfoTooltip()
        {
            if(CurentHoveredLootModel == null || CurrentHoveredItemsGridType == ItemsGridType.QuickItems || CurrentHoveredItemsGridType == ItemsGridType.None)
            {
                LootInfoTooltipModel.SetIsActive(false);
                return;
            }

            LootInfoTooltipModel.SetLootModel(CurentHoveredLootModel);
            LootInfoTooltipModel.SetIsActive(true);
        }

        #endregion
    }
}

