using ShadowFlareRemake.PlayerInputReader;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.InputManagement
{
    public class InputManager : LayersAndTagsReader, IPlayerInputReader
    {
        [Header("Camera")]
        [SerializeField] private Camera _mainCamera;

        [Space(5)]
        [Header("----- Mouse Input Actions -----")]
        [Space(5)]
        [SerializeField] private InputAction LeftMouseClickAction;
        [SerializeField] private InputAction RightMouseClickAction;

        [Space(10)]
        [Header("----- Keyboard Letters Input Actions -----")]
        [Space(5)]
        [SerializeField] private InputAction I_KeyboardClickAction;
        [SerializeField] private InputAction S_KeyboardClickAction;
        [SerializeField] private InputAction H_KeyboardClickAction;

        [Space(10)]
        [Header("----- Keyboard Nums Input Actions -----")]
        [Space(5)]
        [SerializeField] private InputAction NumOne_KeyboardClickAction;
        [SerializeField] private InputAction NumTwo_KeyboardClickAction;
        [SerializeField] private InputAction NumThree_KeyboardClickAction;
        [SerializeField] private InputAction NumFour_KeyboardClickAction;
        [SerializeField] private InputAction NumFive_KeyboardClickAction;
        [SerializeField] private InputAction NumSix_KeyboardClickAction;
        [SerializeField] private InputAction NumSeven_KeyboardClickAction;
        [SerializeField] private InputAction NumEight_KeyboardClickAction;

        [Space(10)]
        [Header("----- Keyboard F Keys Input Actions -----")]
        [Space(5)]
        [SerializeField] private InputAction F1_KeyboardClickAction;
        [SerializeField] private InputAction F2_KeyboardClickAction;
        [SerializeField] private InputAction F3_KeyboardClickAction;
        [SerializeField] private InputAction F4_KeyboardClickAction;
        [SerializeField] private InputAction F5_KeyboardClickAction;
        [SerializeField] private InputAction F6_KeyboardClickAction;
        [SerializeField] private InputAction F7_KeyboardClickAction;
        [SerializeField] private InputAction F8_KeyboardClickAction;
        [SerializeField] private InputAction F9_KeyboardClickAction;

        public Collider CurrentRaycastHitCollider { get; private set; }
        public RaycastHit CurrentRaycastHit { get; private set; }
        public Vector3 CurrentMousePosition { get; private set; }

        public bool IsLeftMouseHeldDown { get; private set; }
        public bool IsHoldingLoot { get; private set; }

        public bool IsCursorOnGround { get; private set; }
        public bool IsCursorOnEnemy { get; private set; }
        public bool IsCursorOnItem { get; private set; }
        public bool IsCursorOnUI { get; private set; }

        private Ray _currentMouseRay;
        private int _raycastLayerMask;
        private bool _isFinishedInit = false;

        private const int _rayCastMaxDistance = 1000;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            InitRaycastLayerMask();
        }

        private void OnEnable()
        {
            SetIsClickActionsEnabled(true);
            _isFinishedInit = true;
        }

        private void Update()
        {
            IsLeftMouseHeldDown = LeftMouseClickAction.IsPressed();
            HandleRaycastHit();
        }

        private void OnDisable()
        {
            SetIsClickActionsEnabled(false);
            _isFinishedInit = false;
        }

        #endregion

        #region Initialization

        private void InitRaycastLayerMask()
        {
            _raycastLayerMask = ~(1 << PlayerLayer);
        }

        public async Task WaitForInitFinish()
        {
            while(!_isFinishedInit)
            {
                await Task.Yield();
            }
        }

        private void SetIsClickActionsEnabled(bool isEnable)
        {
            if(isEnable)
            {
                LeftMouseClickAction.Enable();
                RightMouseClickAction.Enable();

                I_KeyboardClickAction.Enable();
                S_KeyboardClickAction.Enable();
                H_KeyboardClickAction.Enable();

                NumOne_KeyboardClickAction.Enable();
                NumTwo_KeyboardClickAction.Enable();
                NumThree_KeyboardClickAction.Enable();
                NumFour_KeyboardClickAction.Enable();
                NumFive_KeyboardClickAction.Enable();
                NumSix_KeyboardClickAction.Enable();
                NumSeven_KeyboardClickAction.Enable();
                NumEight_KeyboardClickAction.Enable();

                F1_KeyboardClickAction.Enable();
                F2_KeyboardClickAction.Enable();
                F3_KeyboardClickAction.Enable();
                F4_KeyboardClickAction.Enable();
                F5_KeyboardClickAction.Enable();
                F6_KeyboardClickAction.Enable();
                F7_KeyboardClickAction.Enable();
                F8_KeyboardClickAction.Enable();
                F9_KeyboardClickAction.Enable();
            }
            else
            {
                LeftMouseClickAction.Disable();
                RightMouseClickAction.Disable();

                I_KeyboardClickAction.Disable();
                S_KeyboardClickAction.Disable();
                H_KeyboardClickAction.Disable();

                NumOne_KeyboardClickAction.Disable();
                NumTwo_KeyboardClickAction.Disable();
                NumThree_KeyboardClickAction.Disable();
                NumFour_KeyboardClickAction.Disable();
                NumFive_KeyboardClickAction.Disable();
                NumSix_KeyboardClickAction.Disable();
                NumSeven_KeyboardClickAction.Disable();
                NumEight_KeyboardClickAction.Disable();

                F1_KeyboardClickAction.Disable();
                F2_KeyboardClickAction.Disable();
                F3_KeyboardClickAction.Disable();
                F4_KeyboardClickAction.Disable();
                F5_KeyboardClickAction.Disable();
                F6_KeyboardClickAction.Disable();
                F7_KeyboardClickAction.Disable();
                F8_KeyboardClickAction.Disable();
                F9_KeyboardClickAction.Disable();
            }
        }

        #endregion

        #region Meat & Potatos

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

            if(Physics.Raycast(_currentMouseRay, out RaycastHit hit, _rayCastMaxDistance, _raycastLayerMask))
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

        #region Input Actions

        public void ResigterToMouseInputAction(PlayerMouseInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerMouseInputType.LeftMouse:
                    LeftMouseClickAction.performed += action;
                    break;

                case PlayerMouseInputType.RightMouse:
                    RightMouseClickAction.performed += action;
                    break;

                default:
                    break;
            }
        }

        public void DeresigterFromMouseInputAction(PlayerMouseInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerMouseInputType.LeftMouse:
                    LeftMouseClickAction.performed -= action;
                    break;

                case PlayerMouseInputType.RightMouse:
                    RightMouseClickAction.performed -= action;
                    break;

                default:
                    break;
            }
        }

        public void ResigterToKeyboardLettersInputAction(PlayerKeyboardLettersInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerKeyboardLettersInputType.I_Keyboard:
                    I_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardLettersInputType.H_Keyboard:
                    H_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardLettersInputType.S_Keyboard:
                    S_KeyboardClickAction.performed += action;
                    break;

                default:
                    break;
            }
        }

        public void DeresigterFromKeyboardLettersInputAction(PlayerKeyboardLettersInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerKeyboardLettersInputType.I_Keyboard:
                    I_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardLettersInputType.H_Keyboard:
                    H_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardLettersInputType.S_Keyboard:
                    S_KeyboardClickAction.performed -= action;
                    break;

                default:
                    break;
            }
        }

        public void ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerKeyboardNumsInputType.NumOne:
                    NumOne_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumTwo:
                    NumTwo_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumThree:
                    NumThree_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumFour:
                    NumFour_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumFive:
                    NumFive_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumSix:
                    NumSix_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumSeven:
                    NumSeven_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumEight:
                    NumEight_KeyboardClickAction.performed += action;
                    break;

                default:
                    break;
            }
        }

        public void DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerKeyboardNumsInputType.NumOne:
                    NumOne_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumTwo:
                    NumTwo_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumThree:
                    NumThree_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumFour:
                    NumFour_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumFive:
                    NumFive_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumSix:
                    NumSix_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumSeven:
                    NumSeven_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumEight:
                    NumEight_KeyboardClickAction.performed -= action;
                    break;

                default:
                    break;
            }
        }

        public void ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerKeyboardFKeysInputType.F1:
                    F1_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F2:
                    F2_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F3:
                    F3_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F4:
                    F4_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F5:
                    F5_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F6:
                    F6_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F7:
                    F7_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F8:
                    F8_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardFKeysInputType.F9:
                    F9_KeyboardClickAction.performed += action;
                    break;

                default:
                    break;
            }
        }

        public void DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType inputType, Action<InputAction.CallbackContext> action)
        {
            switch(inputType)
            {
                case PlayerKeyboardFKeysInputType.F1:
                    F1_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F2:
                    F2_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F3:
                    F3_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F4:
                    F4_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F5:
                    F5_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F6:
                    F6_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F7:
                    F7_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F8:
                    F8_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardFKeysInputType.F9:
                    F9_KeyboardClickAction.performed -= action;
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
