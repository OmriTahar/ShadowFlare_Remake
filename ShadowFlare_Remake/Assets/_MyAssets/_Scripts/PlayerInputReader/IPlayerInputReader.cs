using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.PlayerInputReader
{
    public interface IPlayerInputReader
    {
        Collider CurrentRaycastHitCollider { get; }
        RaycastHit CurrentRaycastHit { get; }
        Vector3 CurrentMousePosition { get; }

        bool IsCursorOnGround { get; }
        bool IsCursorOnEnemy { get; }
        bool IsCursorOnNPC { get; }
        bool IsCursorOnInteractable { get; }
        bool IsCursorOnItem { get; }
        bool IsCursorOnUI { get; }

        bool IsHoldingLoot { get; }
        bool IsLeftMouseHeldDown { get; }

        void ResigterToMouseInputAction(PlayerMouseInputType inputType, Action<InputAction.CallbackContext> action);
        void DeresigterFromMouseInputAction(PlayerMouseInputType inputType, Action<InputAction.CallbackContext> action);

        void ResigterToKeyboardLettersInputAction(PlayerKeyboardLettersInputType inputType, Action<InputAction.CallbackContext> action);
        void DeresigterFromKeyboardLettersInputAction(PlayerKeyboardLettersInputType inputType, Action<InputAction.CallbackContext> action);

        void ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType inputType, Action<InputAction.CallbackContext> action);
        void DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType inputType, Action<InputAction.CallbackContext> action);

        void ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType inputType, Action<InputAction.CallbackContext> action);
        void DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType inputType, Action<InputAction.CallbackContext> action);
    }
}
