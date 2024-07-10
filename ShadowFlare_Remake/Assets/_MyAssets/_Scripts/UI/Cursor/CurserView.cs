using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Loot;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserView : View<CurserModel>
    {
        public event Action<bool> OnCurserHoldingLootChange;

        [Header("Loot")]
        [SerializeField] private LootView _pickedUpLootView;
        [SerializeField] private Image _pickedUpLootImage;
        [SerializeField] private LootInfoSubView _lootInfoView;

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
            HandleHoveredLoot();
        }

        private void HandlePickedUpLootView()
        {
            var isHoldingLoot = Model.IsHoldingLoot();

            if(!isHoldingLoot)
            {
                _pickedUpLootImage.enabled = false;
            }
            else
            {
                _pickedUpLootView.SetModel(Model.CurrentHeldLootModel);
                _pickedUpLootImage.enabled = true;
            }

            OnCurserHoldingLootChange?.Invoke(isHoldingLoot);
        }

        private void HandleHoveredLoot()
        {
            if(Model.CurentHoveredLootModel == null)
            {
                _lootInfoView.gameObject.SetActive(false);
                return;
            }

            _lootInfoView.gameObject.SetActive(true);
        }

        private void HandleCurserIcon(CursorIconState newCurserIconState)
        {
            var newIcon = GetCurserIcon(newCurserIconState);
            UnityEngine.Cursor.SetCursor(newIcon, Vector2.zero, CursorMode.Auto);
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

