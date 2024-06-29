using UnityEngine;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using System;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserModel : Model
    {
        public LootModel CurentHoveredLootModel { get; private set; }
        public LootModel CurrentHeldLootModel { get; private set; }
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

        public bool IsHoldingLoot()
        {
            return CurrentHeldLootModel != null;
        }

        public void PlaceLootInGrid(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            var tuple = itemsGridModel.TryHandPlaceLootOnGrid(tileIndex, lootModel);
            var isLootPlaced = tuple.Item1;
            var swappedLoot = tuple.Item2;

            if(!isLootPlaced)
                return;

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

        public void DropLootOnGround()
        {
            DropLootLogic();
            Changed();
        }

        private void PickUpLootLogic(LootModel lootModel)
        {
            CurrentHeldLootModel = lootModel;
        }

        private void DropLootLogic()
        {
            CurrentHeldLootModel = null;
        }

        public void SetCurrentHoveredLootModel(LootModel lootModel)
        {
            CurentHoveredLootModel = lootModel;
        }
    }
}

