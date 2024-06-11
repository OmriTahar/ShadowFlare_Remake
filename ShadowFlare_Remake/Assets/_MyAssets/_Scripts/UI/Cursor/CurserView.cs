using UnityEngine;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserView : View<CurserModel>
    {
        [Header("Loot")]
        [SerializeField] private LootView _pickedUpLootView;
        [SerializeField] private Image _pickedUpLootImage;

        [Header("Curser Icons")]
        [SerializeField] private Texture2D _moveCursorIcon;
        [SerializeField] private Texture2D _attackCursorIcon;
        [SerializeField] private Texture2D _pickUpCursorIcon;
        [SerializeField] private Texture2D _uiCursorIcon;
        [SerializeField] private Texture2D _merchantCursorIcon;
        [SerializeField] private Texture2D _npcCursorIcon;
        [SerializeField] private Texture2D _otherCursorIcon;

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
            if(Model.HeldLootModel == null)
            {
                _pickedUpLootImage.enabled = false;
                return;
            }

            _pickedUpLootView.SetModel(Model.HeldLootModel);
            _pickedUpLootImage.enabled = true;
        }

        private Texture2D GetCurserIcon(CursorIconState cursorIconState)
        {
            switch(cursorIconState)
            {
                case CursorIconState.Move:
                    return _moveCursorIcon;

                case CursorIconState.Attack:
                    return _attackCursorIcon;

                case CursorIconState.PickUp:
                    return _pickUpCursorIcon;

                case CursorIconState.UI:
                    return _uiCursorIcon;

                case CursorIconState.Merchacnt:
                    return _merchantCursorIcon;

                case CursorIconState.NPC:
                    return _npcCursorIcon;

                case CursorIconState.Other:
                    return _otherCursorIcon;
            }

            return _otherCursorIcon;
        }
    }
}

