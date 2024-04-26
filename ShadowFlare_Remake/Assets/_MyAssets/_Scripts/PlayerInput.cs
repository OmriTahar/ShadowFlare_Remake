using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake {
    public class PlayerInput : MonoBehaviour {

        public static PlayerInput Instance { get; private set; }

        // --- References ---
        [field: SerializeField] public Camera MainCamera { get; private set; }

        // --- Input Actions ---
        [field: SerializeField] public InputAction LeftMouseClickAction { get; private set; }
        [field: SerializeField] public InputAction RightMouseClickAction { get; private set; }
        [field: SerializeField] public InputAction I_KeyboardClickAction { get; private set; }
        [field: SerializeField] public InputAction H_KeyboardClickAction { get; private set; }

        // --- Variables ---
        public RaycastHit CurrentRaycastHit { get; private set; }
        public bool IsCursorOnUI { get; private set; }
        public bool IsLeftMouseIsHeldDown { get; private set; }

        private Ray _currentMouseRay;

        private void Awake() {

            if(Instance == null) {
                Instance = this;

            } else if(this != Instance) {
                Destroy(this);
            }
        }

        private void OnEnable() {

            LeftMouseClickAction.Enable();
            RightMouseClickAction.Enable();
            I_KeyboardClickAction.Enable();
            H_KeyboardClickAction.Enable();
        }

        private void Update() {

            _currentMouseRay = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(_currentMouseRay, out RaycastHit hit)) {
                CurrentRaycastHit = hit;
            }

            IsLeftMouseIsHeldDown = LeftMouseClickAction.IsPressed();
        }

        private void OnDisable() {

            LeftMouseClickAction.Disable();
            RightMouseClickAction.Disable();
            I_KeyboardClickAction.Disable();
            H_KeyboardClickAction.Disable();
        }

        public void SetIsCursorOnUI(bool isCurserOnUI) {

            IsCursorOnUI = isCurserOnUI;
        }
    }
}
