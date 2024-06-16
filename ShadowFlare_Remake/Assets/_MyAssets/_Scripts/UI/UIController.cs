using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.PlayerInput;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.UI.Cursor;
using ShadowFlareRemake.UI.Hud;
using ShadowFlareRemake.UI.Inventory;
using ShadowFlareRemake.UI.LevelUp;
using ShadowFlareRemake.UI.Stats;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Events;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI
{
    public class UIController : Controller
    {
        public event Action<LootModel> OnDropLootToTheGround;
        public event Action<bool> OnIsCurserOnUiChanged;
        public event Action<bool> OnIsPlayerHoldingLootChanged;

        [Header("Views")]
        [SerializeField] private CurserView _curserView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private StatsView _statsView;
        [SerializeField] private HudView _hudView;
        [SerializeField] private LevelUpView _levelUpView;

        [Header("Loot")]
        [SerializeField] private Transform _pickedUpLootTranform;

        [Header("Other")]
        [SerializeField] private GameObject _closeButton;

        private CurserModel _curserModel;
        private StatsModel _statsModel;
        private InventoryModel _inventoryModel;
        private HudModel _hudModel;
        private LevelUpModel _levelUpModel;

        private IInputManager _inputManager;

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            CacheNulls();
            InitModels();
        }

        private void OnDisable()
        {
            DeregisterEvents();
        }

        private void Update()
        {
            SetCurserIcon();
            SetPickedUpLootPosition();
        }

        #endregion

        #region Initialization

        public void InitUiController(IInputManager inputManager)
        {
            _inputManager = inputManager;
            RegisterEvents();
        }

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

        #region Cursor

        private void SetCurserIcon()
        {
            if(_inputManager.IsCursorOnUI || _curserModel.IsHoldingLoot())
            {
                _curserModel.SetCursorIconState(CursorIconState.UI);
            }
            else if(_inputManager.IsCursorOnGround)
            {
                _curserModel.SetCursorIconState(CursorIconState.Move);
            }
            else if(_inputManager.IsCursorOnEnemy)
            {
                _curserModel.SetCursorIconState(CursorIconState.Attack);
            }
            else if(_inputManager.IsCursorOnItem)
            {
                _curserModel.SetCursorIconState(CursorIconState.PickUp);
            }
            else
                _curserModel.SetCursorIconState(CursorIconState.Other);
        }

        private void CursorEnteredUI(PointerEventData eventData)
        {
            OnIsCurserOnUiChanged?.Invoke(true);
        }

        private void CursorLeftUI(PointerEventData eventData)
        {
            OnIsCurserOnUiChanged?.Invoke(false);
        }

        private void HandleIsCurserHoldingLoot(bool isHoldingLoot)
        {
            DropLootLeftMouseClickEvent(isHoldingLoot);
            OnIsPlayerHoldingLootChanged?.Invoke(isHoldingLoot);
        }

        #endregion

        #region Item Grids & Loot

        public bool TryPickUpLootFromTheGround(LootModel lootModel)
        {
            if(_inventoryModel.IsInventoryOpen)
            {
                _curserModel.PickUpLootFromGround(lootModel);
                return true;
            }

            var specificItemsGridModel = _inventoryModel.GetItemsGridModel(lootModel.LootData.Type);
            var carryItemsGridModel = _inventoryModel.GetItemsGridModel(LootType.All);

            if(specificItemsGridModel.TryAutoPlaceLootOnGrid(lootModel))
            {
                return true;
            }

            if(carryItemsGridModel.TryAutoPlaceLootOnGrid(lootModel))
            {
                return true;
            }

            return false;
        }

        private void DropLootToTheGround(InputAction.CallbackContext context)
        {
            if(!_curserModel.IsHoldingLoot() || _inputManager.IsCursorOnUI)
                return;

            OnDropLootToTheGround?.Invoke(_curserModel.HeldLootModel);
            _curserModel.DropLootOnGround();
        }

        private void SetPickedUpLootPosition()
        {
            _pickedUpLootTranform.position = _inputManager.CurrentMousePosition;
        }

        private void SetCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            if(!isCursorOn)
            {
                _curserModel.SetCurrentHoveredItemsGrid(null);
                return;
            }

            _curserModel.SetCurrentHoveredItemsGrid(itemsGridModel);
        }

        private void HandleItemsGridClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            if(itemsGridModel == null)
                return;

            print($"{itemsGridModel.Name} was clicked at tile index: {tileIndex} | Contains Loot Model: {lootModel != null}");

            var cursorLootModel = _curserModel.HeldLootModel;

            if(cursorLootModel == null && lootModel == null)
                return;

            if(cursorLootModel != null)
            {
                _curserModel.PlaceLootInGrid(itemsGridModel, tileIndex, cursorLootModel);
            }
            else
                _curserModel.PickUpLootFromGrid(itemsGridModel, tileIndex, lootModel);
        }

        #endregion

        #region Inventory

        private void ToggleInventory() // Called from a UI button clicked event
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
            CursorEvents(true);
            HudEvents(true);
            InventoryEvents(true);
            StatsEvents(true);
            LevelUpEvents(true);
        }

        private void DeregisterEvents()
        {
            InputManagerEvents(false);
            CursorEvents(false);
            HudEvents(false);
            InventoryEvents(false);
            StatsEvents(false);
            LevelUpEvents(false);
        }

        private void InputManagerEvents(bool isRegister)
        {
            if(isRegister)
            {
                _inputManager.ResigterToInputAction(PlayerInputType.I_Keyboard, ToggleInventory);
                _inputManager.ResigterToInputAction(PlayerInputType.S_Keyboard, ToggleInventory);
            }
            else
            {
                _inputManager.DeresigterFromInputAction(PlayerInputType.I_Keyboard, ToggleInventory);
                _inputManager.DeresigterFromInputAction(PlayerInputType.S_Keyboard, ToggleInventory);
            }
        }

        private void DropLootLeftMouseClickEvent(bool isRegister)
        {
            if(isRegister)
                _inputManager.ResigterToInputAction(PlayerInputType.LeftMouse, DropLootToTheGround);
            else
                _inputManager.DeresigterFromInputAction(PlayerInputType.LeftMouse, DropLootToTheGround);
        }

        private void CursorEvents(bool isRegister)
        {
            if(isRegister)
                _curserView.OnCurserHoldingLootChange += HandleIsCurserHoldingLoot;
            else
                _curserView.OnCurserHoldingLootChange -= HandleIsCurserHoldingLoot;
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
                _inventoryView.OnTileClicked += HandleItemsGridClicked;
            }
            else
            {
                _inventoryView.OnCurserEnterUI -= CursorEnteredUI;
                _inventoryView.OnCurserLeftUI -= CursorLeftUI;
                _inventoryView.OnCursorChangedHoverOverGrid -= SetCurrentHoveredItemsGrid;
                _inventoryView.OnTileClicked -= HandleItemsGridClicked;
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

