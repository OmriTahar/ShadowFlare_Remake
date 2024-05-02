using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Events;
using ShadowFlareRemake.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI {
    public class UIController : Controller {

        [Header("Views")]
        [SerializeField] private CurserView _curserView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private StatsView _statsView;
        [SerializeField] private HudView _hudView;

        private CurserModel _curserModel;
        private StatsModel _statsModel;
        private InventoryModel _inventoryModel;
        private HudModel _hudModel;

        protected override void Awake() {

            base.Awake();
            CacheNulls();
            InitModels();
        }

        private async void Start() {

            await PlayerInput.Instance.WaitForInitFinish();
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

            _inventoryModel = new InventoryModel(false);
            _inventoryView.SetModel(_inventoryModel);

            _statsModel = new StatsModel(false);
            _statsView.SetModel(_statsModel);

            _hudModel = new HudModel();
            _hudView.SetModel(_hudModel);
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
            _inventoryModel.SetIsInventoryOpen(toggledState);

            //var screenCover = _inventoryModel.IsInventoryOpen ? UIScreenCover.RightIsCovered : UIScreenCover.None;
            //Dispatcher.Dispatch(new UIScreenCoverEvent(screenCover));
        }

        private void ToggleStats() {

            var toggledState = !_statsModel.IsStatsOpen;
            _statsModel.SetIsStatsOpen(toggledState);

            //var screenCover = _statsModel.IsStatsOpen ? UIScreenCover.LeftIsCovered : UIScreenCover.None;
            //Dispatcher.Dispatch(new UIScreenCoverEvent(screenCover));
        }

        private void HandleUiScreenCover() {

            if(_inventoryModel.IsInventoryOpen && _statsModel.IsStatsOpen) {

                // YOU STOPPED HERE!!!
            }

        }

        public void CursorEnteredUI(PointerEventData eventData) {
            PlayerInput.Instance.SetIsCursorOnUI(true);
        }
        public void CursorLeftUI(PointerEventData eventData) {
            PlayerInput.Instance.SetIsCursorOnUI(false);
        }

        #region Update Player Stats

        public void UpdatePlayerStats(IUnit unit) {

            var stats = unit.Stats as IPlayerUnitStats; 

            UpdatePlayerHpAndMp(unit.CurrentHP, stats.MaxHP, unit.CurrentMP, stats.MaxMP);
            UpdatePlayerExp(stats.CurrentExp, stats.ExpToLevelUp);
            UpdatePlayerLevel(stats.Level);
        }

        private void UpdatePlayerHpAndMp(int currentHP, int maxHP, int currentMP, int maxMP) {

            _hudModel.SetHPAndMP(currentHP, maxHP, currentMP, maxMP);
        }

        private void UpdatePlayerExp(int currentExp, int expToLevelUp) {

            _hudModel.SetExp(currentExp, expToLevelUp);
        }

        private void UpdatePlayerLevel(int level) {

            _hudModel.SetLevel(level);
        }

        #endregion

        private void RegisterEvents() {

            //PlayerInput.Instance.I_KeyboardClickAction.performed += ToggleInventory;

            _hudView.OnCurserEnterUI += CursorEnteredUI;
            _hudView.OnCurserLeftUI += CursorLeftUI;
            _hudView.OnInventoryButtonClicked += ToggleInventory;

            _inventoryView.OnCurserEnterUI += CursorEnteredUI;
            _inventoryView.OnCurserLeftUI += CursorLeftUI;
            _inventoryView.OnCloseClicked += ToggleInventory;
        }

        private void DeregisterEvents() {

            //PlayerInput.Instance.I_KeyboardClickAction.performed -= ToggleInventory;

            _hudView.OnCurserEnterUI -= CursorEnteredUI;
            _hudView.OnCurserLeftUI -= CursorLeftUI;
            _hudView.OnInventoryButtonClicked -= ToggleInventory;

            _inventoryView.OnCloseClicked -= ToggleInventory;
            _inventoryView.OnCurserEnterUI -= CursorEnteredUI;
            _inventoryView.OnCurserLeftUI -= CursorLeftUI;
        }
    }
}

