using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserModel : Model
    {
        public LootView PickedUpLootView { get; private set; }
        public LootModel PickedUpLootModel { get; private set; }
        public Transform PickedLootUpTransform { get; private set; }
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

        public void PickUpLootFromGround(LootView lootView)
        {
            PickUpLootLogic(lootView);
            Changed();
        }

        public void PickUpLootFromGrid(Vector2Int tileIndex, LootView lootView, ItemsGridModel itemsGridModel)
        {
            PickUpLootLogic(lootView);
            CurrentHoveredItemsGridModel.RemoveItemFromGrid(tileIndex);
            Changed();
        }

        private void PickUpLootLogic(LootView lootView)
        {
            PickedUpLootView = lootView;

            if(lootView == null)
            {
                PickedLootUpTransform = null;
                return;
            }

            PickedLootUpTransform = lootView.transform;
        }

        private void DropLootLogic()
        {
            PickedUpLootModel = null;
            PickedLootUpTransform = null;
        }

        public void DropLootOnGrid(Vector2Int tileIndex, LootView lootView, ItemsGridModel itemsGridModel)
        {
            itemsGridModel.PlaceLootOnGrid(tileIndex, lootView);
            DropLootLogic();
        }

        public void DropLootOnGround(LootView lootView)
        {
            DropLootLogic();
        }
    }
}

