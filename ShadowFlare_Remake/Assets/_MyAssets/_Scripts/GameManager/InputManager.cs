using ShadowFlareRemake.Enums;
using ShadowFlareRemake.PlayerInput;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.GameManager
{
    public class InputManager : Controller, IInputManager
    {
        [Header("Camera")]
        [SerializeField] private Camera _mainCamera;

        [Space(5)]
        [Header("Mouse Input Actions")]
        [SerializeField] private InputAction LeftMouseClickAction;
        [SerializeField] private InputAction RightMouseClickAction;

        [Space(10)]
        [Header("Keyboard Letters Input Actions")]
        [SerializeField] private InputAction I_KeyboardClickAction;
        [SerializeField] private InputAction S_KeyboardClickAction;
        [SerializeField] private InputAction H_KeyboardClickAction;

        [Space(10)]
        [Header("Keyboard Nums Input Actions")]
        [SerializeField] private InputAction numOne_KeyboardClickAction;
        [SerializeField] private InputAction numTwo_KeyboardClickAction;
        [SerializeField] private InputAction numThree_KeyboardClickAction;
        [SerializeField] private InputAction numFour_KeyboardClickAction;
        [SerializeField] private InputAction numFive_KeyboardClickAction;
        [SerializeField] private InputAction numSix_KeyboardClickAction;
        [SerializeField] private InputAction numSeven_KeyboardClickAction;
        [SerializeField] private InputAction numEight_KeyboardClickAction;

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

                numOne_KeyboardClickAction.Enable();
                numTwo_KeyboardClickAction.Enable();
                numThree_KeyboardClickAction.Enable();
                numFour_KeyboardClickAction.Enable();
                numFive_KeyboardClickAction.Enable();
                numSix_KeyboardClickAction.Enable();
                numSeven_KeyboardClickAction.Enable();
                numEight_KeyboardClickAction.Enable();
            }
            else
            {
                LeftMouseClickAction.Disable();
                RightMouseClickAction.Disable();

                I_KeyboardClickAction.Disable();
                S_KeyboardClickAction.Disable();
                H_KeyboardClickAction.Disable();

                numOne_KeyboardClickAction.Disable();
                numTwo_KeyboardClickAction.Disable();
                numThree_KeyboardClickAction.Disable();
                numFour_KeyboardClickAction.Disable();
                numFive_KeyboardClickAction.Disable();
                numSix_KeyboardClickAction.Disable();
                numSeven_KeyboardClickAction.Disable();
                numEight_KeyboardClickAction.Disable();
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
                    numOne_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumTwo:
                    numTwo_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumThree:
                    numThree_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumFour:
                    numFour_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumFive:
                    numFive_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumSix:
                    numSix_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumSeven:
                    numSeven_KeyboardClickAction.performed += action;
                    break;

                case PlayerKeyboardNumsInputType.NumEight:
                    numEight_KeyboardClickAction.performed += action;
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
                    numOne_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumTwo:
                    numTwo_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumThree:
                    numThree_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumFour:
                    numFour_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumFive:
                    numFive_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumSix:
                    numSix_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumSeven:
                    numSeven_KeyboardClickAction.performed -= action;
                    break;

                case PlayerKeyboardNumsInputType.NumEight:
                    numEight_KeyboardClickAction.performed -= action;
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
