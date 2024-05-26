using UnityEngine;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserView : View<CurserModel>
    {
        [Header("Loot")]
        [SerializeField] private LootView _pickedUpLootView;

        [Header("Curser Icons")]
        [SerializeField] private Texture2D Move;
        [SerializeField] private Texture2D Attack;
        [SerializeField] private Texture2D PickUp;
        [SerializeField] private Texture2D UI;
        [SerializeField] private Texture2D Other;

        protected override void ModelChanged()
        {
            HandleCurserIcon(Model.CurrentCursorIconState);
            HandlePickedUpLootView();
        }

        private void HandleCurserIcon(CursorIconState newCurserIconState)
        {
            var newIcon = GetCurserIcon(newCurserIconState);
            UnityEngine.Cursor.SetCursor(newIcon, Vector2.zero, CursorMode.Auto);
        }

        private void HandlePickedUpLootView()
        {
            _pickedUpLootView.SetModel(Model.PickedUpLootModel);

            if(Model.PickedUpLootModel == null)
            {
                _pickedUpLootView.gameObject.SetActive(false);
                return;
            }

            _pickedUpLootView.gameObject.SetActive(true);
        }

        private Texture2D GetCurserIcon(CursorIconState cursorIconState)
        {
            switch(cursorIconState)
            {
                case CursorIconState.Move:
                    return Move;

                case CursorIconState.Attack:
                    return Attack;

                case CursorIconState.PickUp:
                    return PickUp;

                case CursorIconState.UI:
                    return UI;

                case CursorIconState.Other:
                    return Other;
            }
            return Other;
        }
    }
}

