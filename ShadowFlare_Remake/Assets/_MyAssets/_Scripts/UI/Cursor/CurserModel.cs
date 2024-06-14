using UnityEngine;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserModel : Model
    {
        public LootModel HeldLootModel { get; private set; }
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
            itemsGridModel.RemoveItemFromGrid(tileIndex, true);
            PickUpLootLogic(lootModel);
            Changed();
        }

        private void PickUpLootLogic(LootModel lootModel)
        {
            HeldLootModel = lootModel;
        }

        private void DropLootLogic()
        {
            HeldLootModel = null;
        }

        public void PlaceLootInGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            var tuple = itemsGridModel.TryHandPlaceLootOnGrid(tileIndex, lootModel);
            var isLootPlaced = tuple.Item1;
            var swappedLoot = tuple.Item2;

            if(isLootPlaced)
            {
                if(swappedLoot != null)
                {
                    PickUpLootLogic(swappedLoot);
                }
                else
                {
                    DropLootLogic();
                }

                Changed();
            }
        }

        public void DropLootOnGround()
        {
            DropLootLogic();
        }
    }
}

