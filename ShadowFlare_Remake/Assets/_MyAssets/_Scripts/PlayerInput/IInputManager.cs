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

        void ResigterToInputAction(PlayerInputType inputType, Action<InputAction.CallbackContext> action);
        void DeresigterFromInputAction(PlayerInputType inputType, Action<InputAction.CallbackContext> action);
    }
}
