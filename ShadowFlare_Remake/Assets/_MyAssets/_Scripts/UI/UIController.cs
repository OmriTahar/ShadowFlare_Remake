using UnityEngine;
using UnityEngine.EventSystems;
using ShadowFlareRemake.Player;
using UnityEngine.InputSystem;
using ShadowFlareRemake.Events;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.PlayerInput;
using ShadowFlareRemake.Rewards;

namespace ShadowFlareRemake.UI {
    public class UIController : Controller {

        [Header("Views")]
        [SerializeField] private CurserView _curserView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private StatsView _statsView;
        [SerializeField] private HudView _hudView;
        [SerializeField] private LevelUpView _levelUpView;

        [Header("Other")]
        [SerializeField] private GameObject _closeButton;

        private CurserModel _curserModel;
        private StatsModel _statsModel;
        private InventoryModel _inventoryModel;
        private HudModel _hudModel;
        private LevelUpModel _levelUpModel;

        #region Unity Callbacks

        protected override void Awake() {

            base.Awake();
            CacheNulls();
            InitModels();
        }

        private async void Start() {

            await InputManager.Instance.WaitForInitFinish();
            RegisterEvents();
        }

        private void OnDisable() {

            DeregisterEvents();
        }

        private void Update() {

            HandleMouseRaycastHit();
        }

        #endregion

        #region Initialization

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

            _levelUpModel = new LevelUpModel();
            _levelUpView.SetModel(_levelUpModel);
        }

        #endregion

        #region Mouse Raycast

        private void HandleMouseRaycastHit() {

            if(InputManager.Instance.IsCursorOnUI) {
                _curserModel.UpdateCurser(CurserModel.CursorIconState.UI);
                return;
            }

            var raycastHit = InputManager.Instance.CurrentRaycastHit;
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

        public void CursorEnteredUI(PointerEventData eventData) {

            InputManager.Instance.SetIsCursorOnUI(true);
        }

        public void CursorLeftUI(PointerEventData eventData) {

            InputManager.Instance.SetIsCursorOnUI(false);
        }

        #endregion

        #region Inventory

        private void ToggleInventory() {

            DoToggleInventory();
        }

        private void ToggleInventory(InputAction.CallbackContext context) { 

            DoToggleInventory();
        }

        private void DoToggleInventory() {

            var toggledState = !_inventoryModel.IsInventoryOpen;
            _inventoryModel.SetIsInventoryOpen(toggledState);
            HandleUiScreenCover();
        }

        #endregion

        #region Stats

        private void ToggleStats() {

            DoToggleStats();
        }

        private void ToggleStats(InputAction.CallbackContext context) {

            DoToggleStats();
        }

        private void DoToggleStats() {

            var toggledState = !_statsModel.IsPanelOpen;
            _statsModel.SetIsStatsOpen(toggledState);
            HandleUiScreenCover();
        }

        #endregion

        #region Update Player UI

        public void UpdatePlayerUI(IUnit unit) {

            var stats = unit.Stats as IPlayerUnitStats;

            UpdatePlayerHpAndMp(unit.CurrentHP, stats.MaxHP, unit.CurrentMP, stats.MaxMP);
            UpdatePlayerExp(stats.CurrentExp, stats.ExpToLevelUp);
            UpdatePlayerLevel(stats.Level);
            UpdatePlayerStats(unit);
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

        private void UpdatePlayerStats(IUnit unit) {

            _statsModel.SetPlayerStats(unit);
        }

        private void HandleUiScreenCover() {

            if(_inventoryModel.IsInventoryOpen && _statsModel.IsPanelOpen) {

                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.BothAreCovered));
                _closeButton.SetActive(true);

            } else if(_inventoryModel.IsInventoryOpen && !_statsModel.IsPanelOpen) {

                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.RightIsCovered));
                _closeButton.SetActive(true);

            } else if(!_inventoryModel.IsInventoryOpen && _statsModel.IsPanelOpen) {

                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.LeftIsCovered));
                _closeButton.SetActive(true);

            } else {
                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.None));
                _closeButton.SetActive(false);
            }
        }

        public void CloseClicked() {

            _inventoryModel.SetIsInventoryOpen(false);
            _statsModel.SetIsStatsOpen(false);
            HandleUiScreenCover();
        }

        #endregion

        #region Level Up

        public void ShowLevelUpPopup(int newLevel ,ILevelUpReward reward) {

            _levelUpModel.SetReward(newLevel, reward, true);
        }

        public void CloseLevelUpPopup() {

            _levelUpModel.SetIsPanelOpen(false);
        }

        #endregion

        #region Events

        private void RegisterEvents() {

            InputManager.Instance.I_KeyboardClickAction.performed += ToggleInventory;
            InputManager.Instance.S_KeyboardClickAction.performed += ToggleStats;

            _hudView.OnCurserEnterUI += CursorEnteredUI;
            _hudView.OnCurserLeftUI += CursorLeftUI;
            _hudView.OnInventoryButtonClicked += ToggleInventory;
            _hudView.OnStatsClicked += ToggleStats;

            _inventoryView.OnCurserEnterUI += CursorEnteredUI;
            _inventoryView.OnCurserLeftUI += CursorLeftUI;

            _levelUpView.OnPanelClicked += CloseLevelUpPopup;
        }

        private void DeregisterEvents() {

            InputManager.Instance.I_KeyboardClickAction.performed -= ToggleInventory;
            InputManager.Instance.S_KeyboardClickAction.performed -= ToggleStats;

            _hudView.OnCurserEnterUI -= CursorEnteredUI;
            _hudView.OnCurserLeftUI -= CursorLeftUI;
            _hudView.OnInventoryButtonClicked -= ToggleInventory;
            _hudView.OnStatsClicked -= ToggleStats;

            _inventoryView.OnCurserEnterUI -= CursorEnteredUI;
            _inventoryView.OnCurserLeftUI -= CursorLeftUI;

            _levelUpView.OnPanelClicked -= CloseLevelUpPopup;
        }

        #endregion
    }
}

