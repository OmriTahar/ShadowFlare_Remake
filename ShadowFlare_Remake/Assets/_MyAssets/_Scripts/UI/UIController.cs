using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Events;
using ShadowFlareRemake.Loot;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.PlayerInput;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.UI.Cursor;
using ShadowFlareRemake.UI.Hud;
using ShadowFlareRemake.UI.Inventory;
using ShadowFlareRemake.UI.LevelUp;
using ShadowFlareRemake.UI.Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.UI
{
    public class UIController : Controller
    {
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

        private InputManager _inputManager;

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            CacheNulls();
            InitModels();
        }

        private async void Start()
        {
            _inputManager = InputManager.Instance;
            await InputManager.Instance.WaitForInitFinish();
            RegisterEvents();
        }

        private void OnDisable()
        {
            DeregisterEvents();
        }

        private void Update()
        {
            HandleMouseRaycastHit();
            HandlePickedItem();
        }

        #endregion

        #region Initialization

        private void CacheNulls()
        {
            if(_inventoryView == null)
            {
                _inventoryView = GetComponentInChildren<InventoryView>();
            }
            if(_hudView == null)
            {
                _hudView = GetComponentInChildren<HudView>();
            }
        }

        private void InitModels()
        {
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

        private void HandleMouseRaycastHit()
        {
            if(_inputManager.IsCursorOnUI)
            {
                _curserModel.UpdateCurser(CurserModel.CursorIconState.UI);
            }
            else if(_inputManager.IsCursorOnGround)
            {
                _curserModel.UpdateCurser(CurserModel.CursorIconState.Move);
            }
            else if(_inputManager.IsCursorOnEnemy)
            {
                _curserModel.UpdateCurser(CurserModel.CursorIconState.Attack);
            }
            else if(_inputManager.IsCursorOnItem)
            {
                _curserModel.UpdateCurser(CurserModel.CursorIconState.PickUp);
            }
            else
            {
                _curserModel.UpdateCurser(CurserModel.CursorIconState.Other);
            }
        }

        public void CursorEnteredUI(PointerEventData eventData)
        {
            _inputManager.SetIsCursorOnUI(true);
        }

        public void CursorLeftUI(PointerEventData eventData)
        {
            _inputManager.SetIsCursorOnUI(false);
        }

        #endregion

        #region Inventory

        private void ToggleInventory()
        {
            DoToggleInventory();
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            DoToggleInventory();
        }

        private void DoToggleInventory()
        {
            var toggledState = !_inventoryModel.IsInventoryOpen;
            _inventoryModel.SetIsInventoryOpen(toggledState);
            HandleUiScreenCover();
        }

        private void SetCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            if(!isCursorOn)
            {
                _inventoryModel.SetCurrentHoveredItemsGrid(null);
                return;
            }

            _inventoryModel.SetCurrentHoveredItemsGrid(itemsGridModel);
        }

        private void HandleInventoryItemsGridClicked(Vector2Int tileIndex, LootView lootView)
        {
            var currentHoveredItemsGrid = _inventoryModel.CurrentHoveredItemsGridModel;

            if(currentHoveredItemsGrid == null)
                return;

            print($"{ currentHoveredItemsGrid.Name} was clicked at tile index: {tileIndex} | Is Inventory Item: {lootView != null}");

            if(_inventoryModel.PickedLoot == null && lootView != null)
            {
                _inventoryModel.PickUpLoot(tileIndex, lootView);
            }
            else if(_inventoryModel.PickedLoot != null)
            {
                _inventoryModel.PlaceLootOnGrid(tileIndex, lootView);
            }
        }

        private void HandlePickedItem()
        {
            if(_inventoryModel.PickedLootTransform == null)
                return;

            _inventoryModel.PickedLootTransform.position = _inputManager.CurrentMousePosition;
        }


        #endregion

        #region Stats

        private void ToggleStats()
        {
            DoToggleStats();
        }

        private void ToggleStats(InputAction.CallbackContext context)
        {
            DoToggleStats();
        }

        private void DoToggleStats()
        {
            var toggledState = !_statsModel.IsPanelOpen;
            _statsModel.SetIsStatsOpen(toggledState);
            HandleUiScreenCover();
        }

        #endregion

        #region Update Player UI

        public void UpdatePlayerUI(IUnit unit)
        {
            var stats = unit.Stats as IPlayerUnitStats;

            UpdatePlayerHpAndMp(unit.CurrentHP, stats.MaxHP, unit.CurrentMP, stats.MaxMP);
            UpdatePlayerExp(stats.CurrentExp, stats.ExpToLevelUp);
            UpdatePlayerLevel(stats.Level);
            UpdatePlayerStats(unit);
        }

        private void UpdatePlayerHpAndMp(int currentHP, int maxHP, int currentMP, int maxMP)
        {
            _hudModel.SetHPAndMP(currentHP, maxHP, currentMP, maxMP);
        }

        private void UpdatePlayerExp(int currentExp, int expToLevelUp)
        {
            _hudModel.SetExp(currentExp, expToLevelUp);
        }

        private void UpdatePlayerLevel(int level)
        {
            _hudModel.SetLevel(level);
        }

        private void UpdatePlayerStats(IUnit unit)
        {
            _statsModel.SetPlayerStats(unit);
        }

        private void HandleUiScreenCover()
        {
            if(_inventoryModel.IsInventoryOpen && _statsModel.IsPanelOpen)
            {
                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.BothAreCovered));
                _closeButton.SetActive(true);
            }
            else if(_inventoryModel.IsInventoryOpen && !_statsModel.IsPanelOpen)
            {
                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.RightIsCovered));
                _closeButton.SetActive(true);
            }
            else if(!_inventoryModel.IsInventoryOpen && _statsModel.IsPanelOpen)
            {
                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.LeftIsCovered));
                _closeButton.SetActive(true);
            }
            else
            {
                Dispatcher.Dispatch(new UIScreenCoverEvent(UIScreenCover.None));
                _closeButton.SetActive(false);
            }
        }

        public void CloseClicked()
        {
            _inventoryModel.SetIsInventoryOpen(false);
            _statsModel.SetIsStatsOpen(false);
            HandleUiScreenCover();
        }

        #endregion

        #region Level Up

        public void ShowLevelUpPopup(int newLevel, ILevelUpReward reward)
        {
            _levelUpModel.SetReward(newLevel, reward);
        }

        public void HandleLevelUpPanelClicked()
        {
            switch(_levelUpModel.State)
            {
                case LevelUpModel.LevelUpPanelState.Hidden:
                    _levelUpModel.SetPanelState(LevelUpModel.LevelUpPanelState.Idle);
                    break;

                case LevelUpModel.LevelUpPanelState.Idle:
                    _levelUpModel.SetPanelState(LevelUpModel.LevelUpPanelState.Corner);
                    break;

                case LevelUpModel.LevelUpPanelState.Corner:
                    _levelUpModel.SetPanelState(LevelUpModel.LevelUpPanelState.Hidden);
                    break;

                default:
                    break;
            }

        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            InputManagerEvents(true);
            HudEvents(true);
            InventoryEvents(true);
            StatsEvents(true);
            LevelUpEvents(true);
        }

        private void DeregisterEvents()
        {
            InputManagerEvents(false);
            HudEvents(false);
            InventoryEvents(false);
            StatsEvents(false);
            LevelUpEvents(false);
        }

        private void InputManagerEvents(bool isRegister)
        {
            if(isRegister)
            {
                _inputManager.I_KeyboardClickAction.performed += ToggleInventory;
                _inputManager.S_KeyboardClickAction.performed += ToggleStats;
            }
            else
            {
                _inputManager.I_KeyboardClickAction.performed -= ToggleInventory;
                _inputManager.S_KeyboardClickAction.performed -= ToggleStats;
            }
        }

        private void HudEvents(bool isRegister)
        {
            if(isRegister)
            {
                _hudView.OnCurserEnterUI += CursorEnteredUI;
                _hudView.OnCurserLeftUI += CursorLeftUI;
                _hudView.OnInventoryButtonClicked += ToggleInventory;
                _hudView.OnStatsClicked += ToggleStats;
            }
            else
            {
                _hudView.OnCurserEnterUI -= CursorEnteredUI;
                _hudView.OnCurserLeftUI -= CursorLeftUI;
                _hudView.OnInventoryButtonClicked -= ToggleInventory;
                _hudView.OnStatsClicked -= ToggleStats;
            }
        }

        private void InventoryEvents(bool isRegister)
        {
            if(isRegister)
            {
                _inventoryView.OnCurserEnterUI += CursorEnteredUI;
                _inventoryView.OnCurserLeftUI += CursorLeftUI;
                _inventoryView.OnCursorChangedHoverOverGrid += SetCurrentHoveredItemsGrid;
                _inventoryView.OnTileClicked += HandleInventoryItemsGridClicked;
            }
            else
            {
                _inventoryView.OnCurserEnterUI -= CursorEnteredUI;
                _inventoryView.OnCurserLeftUI -= CursorLeftUI;
                _inventoryView.OnCursorChangedHoverOverGrid -= SetCurrentHoveredItemsGrid;
                _inventoryView.OnTileClicked -= HandleInventoryItemsGridClicked;
            }
        }

        private void StatsEvents(bool isRegister)
        {
            if(isRegister)
            {
                _statsView.OnCurserEnterUI += CursorEnteredUI;
                _statsView.OnCurserLeftUI += CursorLeftUI;
            }
            else
            {
                _statsView.OnCurserEnterUI -= CursorEnteredUI;
                _statsView.OnCurserLeftUI -= CursorLeftUI;
            }
        }

        private void LevelUpEvents(bool isRegister)
        {
            if(isRegister)
            {
                _levelUpView.OnCurserEnterUI += CursorEnteredUI;
                _levelUpView.OnCurserLeftUI += CursorLeftUI;
                _levelUpView.OnPanelClicked += HandleLevelUpPanelClicked;
            }
            else
            {
                _levelUpView.OnCurserEnterUI -= CursorEnteredUI;
                _levelUpView.OnCurserLeftUI -= CursorLeftUI;
                _levelUpView.OnPanelClicked -= HandleLevelUpPanelClicked;
            }
        }

        #endregion
    }
}

