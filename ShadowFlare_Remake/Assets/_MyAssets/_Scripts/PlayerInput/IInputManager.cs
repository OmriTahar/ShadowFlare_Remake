using System;
using UnityEngine;
using UnityEngine.InputSystem;
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.PlayerInput
{
    public interface IInputManager
    {
        Collider CurrentRaycastHitCollider { get; }
        RaycastHit CurrentRaycastHit { get; }
        Vector3 CurrentMousePosition { get; }

        bool IsCursorOnGround { get; }
        bool IsCursorOnEnemy { get; }
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
    }
}
