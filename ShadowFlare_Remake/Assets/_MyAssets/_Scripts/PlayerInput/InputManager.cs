using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.PlayerInput {
    public class InputManager : Controller {

        public static InputManager Instance { get; private set; }

        // --- References ---
        [field: SerializeField] public Camera MainCamera { get; private set; }

        // --- Input Actions ---
        [field: SerializeField] public InputAction LeftMouseClickAction { get; private set; }
        [field: SerializeField] public InputAction RightMouseClickAction { get; private set; }
        [field: SerializeField] public InputAction I_KeyboardClickAction { get; private set; }
        [field: SerializeField] public InputAction S_KeyboardClickAction { get; private set; }
        [field: SerializeField] public InputAction H_KeyboardClickAction { get; private set; }

        // --- Variables ---
        public RaycastHit CurrentRaycastHit { get; private set; }
        public bool IsCursorOnUI { get; private set; }
        public bool IsCursorOnEnemy { get; private set; }
        public bool IsLeftMouseIsHeldDown { get; private set; }

        private Ray _currentMouseRay;
        private bool _isFinishedInit = false;

        protected override void Awake() {

            base.Awake();
            InitSingleton();
        }

        private void OnEnable() {

            LeftMouseClickAction.Enable();
            RightMouseClickAction.Enable();
            I_KeyboardClickAction.Enable();
            S_KeyboardClickAction.Enable();
            H_KeyboardClickAction.Enable();

            _isFinishedInit = true;
        }

        private void Update() {

            IsLeftMouseIsHeldDown = LeftMouseClickAction.IsPressed();
            HandleRaycastHit();
        }

        private void OnDisable() {

            LeftMouseClickAction.Disable();
            RightMouseClickAction.Disable();
            I_KeyboardClickAction.Disable();
            S_KeyboardClickAction.Disable();
            H_KeyboardClickAction.Disable();

            _isFinishedInit = false;
        }

        public async Task WaitForInitFinish() {

            while(!_isFinishedInit) {

                await Task.Yield();
            }
        }

        public void SetIsCursorOnUI(bool isCurserOnUI) {

            IsCursorOnUI = isCurserOnUI;
        }

        private void InitSingleton() {

            if(Instance == null) {
                Instance = this;

            } else if(this != Instance) {
                Destroy(this);
            }
        }

        private void HandleRaycastHit() {

            _currentMouseRay = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if(Physics.Raycast(_currentMouseRay, out RaycastHit hit)) {

                CurrentRaycastHit = hit;

                var raycastLayer = hit.collider.gameObject.layer;

                IsCursorOnEnemy = raycastLayer.CompareTo(EnemyLayer) == 0;
            }
        }
    }
}