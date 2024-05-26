using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserModel : Model
    {
        public LootModel PickedUpLootModel { get; private set; }
        public ItemsGridModel CurrentHoveredItemsGridModel { get; private set; }

        public CursorIconState CurrentCursorIconState { get; private set; }

        public CurserModel(CursorIconState cursorState = CursorIconState.Move)
        {
            SetCursorIconState(cursorState);
        }

        public void SetCursorIconState(CursorIconState newCursorState)
        {
            if(newCursorState == CurrentCursorIconState)
                return;

            CurrentCursorIconState = newCursorState;
            Changed();
        }

        public void SetCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel)
        {
            CurrentHoveredItemsGridModel = itemsGridModel;
        }

        public void PickUpLootFromGround(LootModel lootModel)
        {
            PickUpLootLogic(lootModel);
            Changed();
        }

        public void PickUpLootFromGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            PickUpLootLogic(lootModel);
            CurrentHoveredItemsGridModel.RemoveItemFromGrid(tileIndex);
            Changed();
        }

        private void PickUpLootLogic(LootModel lootModel)
        {
            PickedUpLootModel = lootModel;
        }

        private void DropLootLogic()
        {
            PickedUpLootModel = null;
        }

        public void DropLootOnGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            itemsGridModel.PlaceLootOnGrid(tileIndex, lootModel);
            DropLootLogic();
        }

        public void DropLootOnGround()
        {
            DropLootLogic();
        }
    }
}

