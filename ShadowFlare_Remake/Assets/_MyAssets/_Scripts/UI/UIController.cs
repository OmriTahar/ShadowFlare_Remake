using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Events;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI {
    public class UIController : Controller {

        [Header("Views")]
        [SerializeField] private CurserView _curserView;
        [SerializeField] private HudView _hudView;
        [SerializeField] private InventoryView _inventoryView;

        [Header("Other")]
        [SerializeField] private Unit _playerUnit;

        private CurserModel _curserModel;
        private HudModel _hudModel;
        private InventoryModel _inventoryModel;

        protected void Awake() {

            base.Init();
            CacheNulls();
        }

        private void Start() {
            InitModels();
        }

        private async void OnEnable() {

            await AwaitPlayerInputInit();
            RegisterEvents();
        }

        private void OnDisable() {
            DeregisterEvents();
        }

        private void Update() {
            HandleMouseRaycastHit();
        }

        private void CacheNulls() {

            if(_inventoryView == null) {
                _inventoryView = GetComponentInChildren<InventoryView>();
            }
            if(_hudView == null) {
                _hudView = GetComponentInChildren<HudView>();
            }
        }

        private void InitModels() {

            _curserModel = new CurserModel();
            _curserView.SetModel(_curserModel);

            _hudModel = new HudModel(_playerUnit.Stats);
            _hudView.SetModel(_hudModel);

            _inventoryModel = new InventoryModel(false);
            _inventoryView.SetModel(_inventoryModel);
        }

        private void HandleMouseRaycastHit() {

            if(PlayerInput.Instance.IsCursorOnUI) {
                _curserModel.UpdateCurser(CurserModel.CursorIconState.UI);
                return;
            }

            var raycastHit = PlayerInput.Instance.CurrentRaycastHit;
            if(raycastHit.collider) {

                if(raycastHit.collider.gameObject.layer.CompareTo(GroundLayer) == 0) {
                    _curserModel.UpdateCurser(CurserModel.CursorIconState.Move);

                } else if(raycastHit.collider.gameObject.layer.CompareTo(EnemyLayer) == 0) {
                    _curserModel.UpdateCurser(CurserModel.CursorIconState.Attack);

                } else if(raycastHit.collider.gameObject.layer.CompareTo(ItemLayer) == 0) {
                    _curserModel.UpdateCurser(CurserModel.CursorIconState.PickUp);

                } else {
                    _curserModel.UpdateCurser(CurserModel.CursorIconState.Other);
                }
            }
        }

        //private void ToggleInventory(InputAction.CallbackContext context) { // For "I" Clicked
        //    DoToggleInventory();
        //}

        private void ToggleInventory() {

            var toggledState = !_inventoryModel.IsInventoryOpen;
            _inventoryModel.UpdateInventory(toggledState);

            var screenCover = _inventoryModel.IsInventoryOpen ? UIScreenCover.RightIsCovered : UIScreenCover.None;
            Dispatcher.Dispatch(new UIScreenCoverEvent(screenCover));
        }

        //private void HurtPlayer(InputAction.CallbackContext context) {
        //    _hudModel.UpdatePlayerStats(_hudModel.MaxHealth, _hudModel.CurrentHealth - 10);
        //}

        public void CursorEnteredUI(PointerEventData eventData) {
            PlayerInput.Instance.SetIsCursorOnUI(true);
        }
        public void CursorLeftUI(PointerEventData eventData) {
            PlayerInput.Instance.SetIsCursorOnUI(false);
        }

        #region Update Player Stats
        public void UpdatePlayerHPAndMP() {
            _hudModel.SetHPAndMP(_playerUnit.CurrentHP, _playerUnit.CurrentMP);
        }

        public void UpdatePlayerExp() {
            _hudModel.SetExp(_playerUnit.Stats.CurrentExp, _playerUnit.Stats.ExpToLevelUp);
        }

        public void UpdatePlayerLevel() {
            _hudModel.SetLevel(_playerUnit.Stats.Level);
        }
        #endregion

        private void RegisterEvents() {

            //PlayerInput.Instance.I_KeyboardClickAction.performed += ToggleInventory;
            //PlayerInput.Instance.H_KeyboardClickAction.performed += HurtPlayer;

            _hudView.OnCurserEnterUI += CursorEnteredUI;
            _hudView.OnCurserLeftUI += CursorLeftUI;
            _hudView.OnInventoryButtonClicked += ToggleInventory;

            _inventoryView.OnCurserEnterUI += CursorEnteredUI;
            _inventoryView.OnCurserLeftUI += CursorLeftUI;
            _inventoryView.OnCloseClicked += ToggleInventory;
        }

        private void DeregisterEvents() {

            //PlayerInput.Instance.I_KeyboardClickAction.performed -= ToggleInventory;
            //PlayerInput.Instance.H_KeyboardClickAction.performed -= HurtPlayer;

            _hudView.OnCurserEnterUI -= CursorEnteredUI;
            _hudView.OnCurserLeftUI -= CursorLeftUI;
            _hudView.OnInventoryButtonClicked -= ToggleInventory;

            _inventoryView.OnCloseClicked -= ToggleInventory;
            _inventoryView.OnCurserEnterUI -= CursorEnteredUI;
            _inventoryView.OnCurserLeftUI -= CursorLeftUI;
        }

        private async Task AwaitPlayerInputInit() {

            while(PlayerInput.Instance == null || PlayerInput.Instance.enabled == false) {
                await Task.Delay(100);
            }
        }
    }
}

