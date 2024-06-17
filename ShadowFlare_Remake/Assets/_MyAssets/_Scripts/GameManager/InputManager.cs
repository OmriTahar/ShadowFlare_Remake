using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.PlayerInput;

namespace ShadowFlareRemake.GameManager
{
    public class InputManager : Controller, IInputManager
    {
        [Header("Camera")]
        [SerializeField] private Camera _mainCamera;

        [Header("Input Actions")]
        [SerializeField] private InputAction LeftMouseClickAction;
        [SerializeField] private InputAction RightMouseClickAction;
        [SerializeField] private InputAction I_KeyboardClickAction;
        [SerializeField] private InputAction S_KeyboardClickAction;
        [SerializeField] private InputAction H_KeyboardClickAction;

        public Collider CurrentRaycastHitCollider { get; private set; }
        public RaycastHit CurrentRaycastHit { get; private set; }
        public Vector3 CurrentMousePosition { get; private set; }

        public bool IsCursorOnGround { get; private set; }
        public bool IsCursorOnEnemy { get; private set; }
        public bool IsCursorOnItem { get; private set; }
        public bool IsCursorOnUI { get; private set; }

        public bool IsHoldingLoot { get; private set; }
        public bool IsLeftMouseHeldDown { get; private set; }

        private Ray _currentMouseRay;
        private int _raycastLayerMask;
        private bool _isFinishedInit = false;

        private const int _rayCastMaxDistance = 1000;

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            LeftMouseClickAction.Enable();
            RightMouseClickAction.Enable();
            I_KeyboardClickAction.Enable();
            S_KeyboardClickAction.Enable();
            H_KeyboardClickAction.Enable();

            _isFinishedInit = true;
        }

        private void Update()
        {
            IsLeftMouseHeldDown = LeftMouseClickAction.IsPressed();
            HandleRaycastHit();
        }

        private void OnDisable()
        {
            LeftMouseClickAction.Disable();
            RightMouseClickAction.Disable();
            I_KeyboardClickAction.Disable();
            S_KeyboardClickAction.Disable();
            H_KeyboardClickAction.Disable();

            _isFinishedInit = false;
        }

        #endregion

        #region Initialization

        public async Task WaitForInitFinish()
        {
            while(!_isFinishedInit)
            {
                await Task.Yield();
            }
        }

        #endregion

        #region Meat & Potatos

        public void ResigterToInputAction(PlayerInputType clickAction, Action<InputAction.CallbackContext> action)
        {
            switch(clickAction)
            {
                case PlayerInputType.LeftMouse:
                    LeftMouseClickAction.performed += action;
                    break;

                case PlayerInputType.RightMouse:
                    RightMouseClickAction.performed += action;
                    break;

                case PlayerInputType.I_Keyboard:
                    I_KeyboardClickAction.performed += action;
                    break;

                case PlayerInputType.H_Keyboard:
                    H_KeyboardClickAction.performed += action;
                    break;

                case PlayerInputType.S_Keyboard:
                    S_KeyboardClickAction.performed += action;
                    break;

                default:
                    break;
            }
        }

        public void DeresigterFromInputAction(PlayerInputType clickAction, Action<InputAction.CallbackContext> action)
        {
            switch(clickAction)
            {
                case PlayerInputType.LeftMouse:
                    LeftMouseClickAction.performed -= action;
                    break;

                case PlayerInputType.RightMouse:
                    RightMouseClickAction.performed -= action;
                    break;

                case PlayerInputType.I_Keyboard:
                    I_KeyboardClickAction.performed -= action;
                    break;

                case PlayerInputType.H_Keyboard:
                    H_KeyboardClickAction.performed -= action;
                    break;

                case PlayerInputType.S_Keyboard:
                    S_KeyboardClickAction.performed -= action;
                    break;

                default:
                    break;
            }
        }

        public void SetIsCursorOnUI(bool isCurserOnUI)
        {
            IsCursorOnUI = isCurserOnUI;
        }

        public void SetIsHoldingLoot(bool isHoldingLoot)
        {
            IsHoldingLoot = isHoldingLoot;
        }

        private void HandleRaycastHit()
        {
            var mousePos = Mouse.current.position.ReadValue();

            CurrentMousePosition = mousePos;
            _currentMouseRay = _mainCamera.ScreenPointToRay(mousePos);

            if(Physics.Raycast(_currentMouseRay, out RaycastHit hit, _rayCastMaxDistance))
            {
                CurrentRaycastHit = hit;
                CurrentRaycastHitCollider = hit.collider;

                var raycastLayer = hit.collider.gameObject.layer;

                IsCursorOnGround = raycastLayer.CompareTo(GroundLayer) == 0;
                IsCursorOnEnemy = raycastLayer.CompareTo(EnemyLayer) == 0;
                IsCursorOnItem = raycastLayer.CompareTo(ItemLayer) == 0;
            }
        }

        #endregion
    }
}
