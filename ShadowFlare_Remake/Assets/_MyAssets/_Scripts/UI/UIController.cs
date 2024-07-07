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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.UI
{
    public class UIController : Controller
    {
        public event Action<LootModel, Vector2Int> OnPotionClicked;
        public event Action<LootModel> OnDropLootToTheGround;
        public event Action<bool> OnIsCurserOnUiChanged;
        public event Action<bool> OnIsPlayerHoldingLootChanged;
        public event Action<List<EquipmentData_ScriptableObject>> OnPlayerGearChanged;

        [Header("Views")]
        [SerializeField] private CurserView _curserView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private StatsView _statsView;
        [SerializeField] private HudView _hudView;
        [SerializeField] private LevelUpView _levelUpView;

        [Header("Loot")]
        [SerializeField] private Transform _pickedUpLootTranform;

        [Header("Other")]
        [SerializeField] private GameObject _closeButton; // Can this be somewhere else?

        private CurserModel _curserModel;
        private StatsModel _statsModel;
        private InventoryModel _inventoryModel;
        private HudModel _hudModel;
        private LevelUpModel _levelUpModel;
        private IInputManager _inputManager;

        #region Quick Items Variables

        private Dictionary<string, Vector2Int> _quickItemsIndexesDict = new();

        private const string _numOne_QuickItemActionName = "Num One_Keyboard Click";
        private const string _numTwo_QuickItemActionName = "Num Two_Keyboard Click";
        private const string _numThree_QuickItemActionName = "Num Three_Keyboard Click";
        private const string _numFour_QuickItemActionName = "Num Four_Keyboard Click";
        private const string _numFive_QuickItemActionName = "Num Five_Keyboard Click";
        private const string _numSix_QuickItemActionName = "Num Six_Keyboard Click";
        private const string _numSeven_QuickItemActionName = "Num Seven_Keyboard Click";
        private const string _numEight_QuickItemActionName = "Num Eight_Keyboard Click";

        private readonly Vector2Int _numOne_QuickItemIndex = new Vector2Int(0, 0);
        private readonly Vector2Int _numTwo_QuickItemIndex = new Vector2Int(0, 1);
        private readonly Vector2Int _numThree_QuickItemIndex = new Vector2Int(1, 0);
        private readonly Vector2Int _numFour_QuickItemIndex = new Vector2Int(1, 1);
        private readonly Vector2Int _numFive_QuickItemIndex = new Vector2Int(2, 0);
        private readonly Vector2Int _numSix_QuickItemIndex = new Vector2Int(2, 1);
        private readonly Vector2Int _numSeven_QuickItemIndex = new Vector2Int(3, 0);
        private readonly Vector2Int _numEight_QuickItemIndex = new Vector2Int(3, 1);

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            CacheNulls();
            InitModels();
            InitQuickItemsIndexesDict();
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

        private void InitQuickItemsIndexesDict()
        {
            _quickItemsIndexesDict.Add(_numOne_QuickItemActionName, _numOne_QuickItemIndex);
            _quickItemsIndexesDict.Add(_numTwo_QuickItemActionName, _numTwo_QuickItemIndex);
            _quickItemsIndexesDict.Add(_numThree_QuickItemActionName, _numThree_QuickItemIndex);
            _quickItemsIndexesDict.Add(_numFour_QuickItemActionName, _numFour_QuickItemIndex);
            _quickItemsIndexesDict.Add(_numFive_QuickItemActionName, _numFive_QuickItemIndex);
            _quickItemsIndexesDict.Add(_numSix_QuickItemActionName, _numSix_QuickItemIndex);
            _quickItemsIndexesDict.Add(_numSeven_QuickItemActionName, _numSeven_QuickItemIndex);
            _quickItemsIndexesDict.Add(_numEight_QuickItemActionName, _numEight_QuickItemIndex);
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
                _curserModel.PickUpLoot(lootModel);
                return true;
            }

            return _inventoryModel.TryAutoPlaceLootOnGrid(lootModel);
        }

        private void DropLootToTheGround(InputAction.CallbackContext context)
        {
            if(!_curserModel.IsHoldingLoot() || _inputManager.IsCursorOnUI)
                return;

            OnDropLootToTheGround?.Invoke(_curserModel.CurrentHeldLootModel);
            _curserModel.DropLoot();
        }

        private void HandleLootViewHovered(LootModel lootModel, Vector2Int tileIndex)
        {
            _curserModel.SetCurrentHoveredLootModel(lootModel, tileIndex);
        }

        private void HandleItemsGridClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            var cursorLootModel = _curserModel.CurrentHeldLootModel;

            if(itemsGridModel == null || (cursorLootModel == null && lootModel == null))
                return;

            if(cursorLootModel != null)
            {
                var tuple = _inventoryModel.TryHandPlaceLootOnGrid(itemsGridModel, tileIndex, cursorLootModel);
                var isLootPlaced = tuple.Item1;
                var swappedLoot = tuple.Item2;

                if(!isLootPlaced)
                    return;

                _curserModel.DropLoot();

                if(swappedLoot != null)
                {
                    _curserModel.PickUpLoot(swappedLoot);
                }
            }
            else
            {
                _inventoryModel.RemoveItemFromGrid(itemsGridModel, tileIndex);
                _curserModel.PickUpLoot(lootModel);
            }

            if(_inventoryModel.IsEquippableItemsGrid(itemsGridModel.ItemsGridType))
            {
                OnPlayerGearChanged?.Invoke(_inventoryModel.CurrentlyEquippedGearData);
            }
        }

        private void SetPickedUpLootPosition()
        {
            _pickedUpLootTranform.position = _inputManager.CurrentMousePosition;
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

        private void HandleKeyboardNumClicked(InputAction.CallbackContext context)
        {
            var index = _quickItemsIndexesDict[context.action.name];
            var lootModel = _inventoryModel.GetQuickItemLootModel(index);

            if(lootModel == null)
                return;

            OnPotionClicked?.Invoke(lootModel, index);
        }

        private void HandleMouseRightClickOnLoot(InputAction.CallbackContext context)
        {
            if(_curserModel.CurentHoveredLootModel == null)
                return;

            OnPotionClicked?.Invoke(_curserModel.CurentHoveredLootModel, _curserModel.CurrentHoveredLootModelRootIndex);
        }

        public void RemovePotionFromInventory(Vector2Int index, LootType lootType)
        {
            _inventoryModel.RemovePotionFromInventory(index, lootType);
        }

        public List<EquipmentData_ScriptableObject> GetPlayerCurrentlyEquippedGearData()
        {
            return _inventoryModel.CurrentlyEquippedGearData;
        }

        #endregion

        #region Stats Panel

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

        #region Update Player Stats & HUD

        public void UpdatePlayerFullUI(IUnit unit, IEquippedGearAddedStats addedStats)
        {
            var stats = unit.Stats as IPlayerUnitStats;

            UpdatePlayerVitals(unit.CurrentHP, stats.MaxHP, unit.CurrentMP, stats.MaxMP);
            UpdatePlayerExp(stats.CurrentExp, stats.ExpToLevelUp);
            UpdatePlayerLevel(stats.Level);
            UpdateFullPlayerStats(unit, addedStats);
        }

        public void UpdatePlayerVitalsAndExp(IUnit unit)
        {
            var stats = unit.Stats as IPlayerUnitStats;

            UpdatePlayerVitals(unit.CurrentHP, stats.MaxHP, unit.CurrentMP, stats.MaxMP);
            UpdatePlayerExp(stats.CurrentExp, stats.ExpToLevelUp);
            UpdatePlayerStats(unit);
        }

        public void UpdatePlayerVitals(int currentHP, int maxHP, int currentMP, int maxMP)
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

        private void UpdateFullPlayerStats(IUnit unit, IEquippedGearAddedStats addedStats)
        {
            _statsModel.SetFullPlayerStats(unit, addedStats);
        }

        private void UpdatePlayerStats(IUnit unit)
        {
            _statsModel.SetPlayerStats(unit);
        }

        #endregion

        #region UI Screen Cover

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

        public void CloseClicked() // Called from a UI button clicked event
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
                case LevelUpModel.LevelUpPanelState.Shown:
                    _levelUpModel.SetPanelState(LevelUpModel.LevelUpPanelState.MovingToCorner);
                    break;

                case LevelUpModel.LevelUpPanelState.MovingToCorner:
                    _levelUpModel.SetPanelState(LevelUpModel.LevelUpPanelState.FadingOut);
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
                // Mouse
                _inputManager.ResigterToMouseInputAction(PlayerMouseInputType.RightMouse, HandleMouseRightClickOnLoot);

                // Keyboard Letters
                _inputManager.ResigterToKeyboardLettersInputAction(PlayerKeyboardLettersInputType.I_Keyboard, ToggleInventory);
                _inputManager.ResigterToKeyboardLettersInputAction(PlayerKeyboardLettersInputType.S_Keyboard, ToggleStats);

                // Keyboard Nums
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumOne, HandleKeyboardNumClicked);
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumTwo, HandleKeyboardNumClicked);
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumThree, HandleKeyboardNumClicked);
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFour, HandleKeyboardNumClicked);
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFive, HandleKeyboardNumClicked);
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSix, HandleKeyboardNumClicked);
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSeven, HandleKeyboardNumClicked);
                _inputManager.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumEight, HandleKeyboardNumClicked);
            }
            else
            {
                // Mouse
                _inputManager.DeresigterFromMouseInputAction(PlayerMouseInputType.RightMouse, HandleMouseRightClickOnLoot);

                // Keyboard Letters
                _inputManager.DeresigterFromKeyboardLettersInputAction(PlayerKeyboardLettersInputType.I_Keyboard, ToggleInventory);
                _inputManager.DeresigterFromKeyboardLettersInputAction(PlayerKeyboardLettersInputType.S_Keyboard, ToggleStats);

                // Keyboard Nums
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumOne, HandleKeyboardNumClicked);
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumTwo, HandleKeyboardNumClicked);
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumThree, HandleKeyboardNumClicked);
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFour, HandleKeyboardNumClicked);
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFive, HandleKeyboardNumClicked);
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSix, HandleKeyboardNumClicked);
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSeven, HandleKeyboardNumClicked);
                _inputManager.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumEight, HandleKeyboardNumClicked);
            }
        }

        private void DropLootLeftMouseClickEvent(bool isRegister)
        {
            if(isRegister)
                _inputManager.ResigterToMouseInputAction(PlayerMouseInputType.LeftMouse, DropLootToTheGround);
            else
                _inputManager.DeresigterFromMouseInputAction(PlayerMouseInputType.LeftMouse, DropLootToTheGround);
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
                _inventoryView.OnTileHovered += HandleLootViewHovered;
                _inventoryView.OnTileClicked += HandleItemsGridClicked;
            }
            else
            {
                _inventoryView.OnCurserEnterUI -= CursorEnteredUI;
                _inventoryView.OnCurserLeftUI -= CursorLeftUI;
                _inventoryView.OnTileHovered -= HandleLootViewHovered;
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

