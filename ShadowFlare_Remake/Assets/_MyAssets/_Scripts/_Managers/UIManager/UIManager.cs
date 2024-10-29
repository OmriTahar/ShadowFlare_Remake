using ShadowFlareRemake.Loot;
using ShadowFlareRemake.NPC;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.PlayerInputReader;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.Skills;
using ShadowFlareRemake.UI;
using ShadowFlareRemake.UI.Cursor;
using ShadowFlareRemake.UI.Dialog;
using ShadowFlareRemake.UI.Hud;
using ShadowFlareRemake.UI.Inventory;
using ShadowFlareRemake.UI.ItemsGrid;
using ShadowFlareRemake.UI.LevelUp;
using ShadowFlareRemake.UI.Stats;
using ShadowFlareRemake.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.UIManagement
{
    public class UIManager : LayersAndTagsReader
    {
        public event Action<List<EquipmentData_ScriptableObject>> OnPlayerGearChanged;
        public event Action<bool> OnIsCurserOnUiChanged;
        public event Action<bool> OnIsPlayerHoldingLootChanged;
        public event Action<LootModel> OnDropLootToTheGround;
        public event Action<LootModel, Vector2Int> OnPotionClicked;
        public event Action<ISkillData> OnHudSkillItemClicked;
        public event Action<UIScreenCover> OnUIScreenCoverChange;

        [Header("Views")]
        [SerializeField] private CurserView _curserView;
        [SerializeField] private DialogView _dialogView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private StatsView _statsView;
        [SerializeField] private HudView _hudView;
        [SerializeField] private LevelUpView _levelUpView;

        [Header("Loot")]
        [SerializeField] private Transform _pickedUpLootTranform;
        [SerializeField] private RectTransform _lootInfoRect;

        private CurserModel _curserModel;
        private DialogModel _dialogModel;
        private StatsModel _statsModel;
        private InventoryModel _inventoryModel;
        private HudModel _hudModel;
        private LevelUpModel _levelUpModel;

        private IPlayerInputReader _inputReader;
        private float _screenSizeX;
        private float _screenSizeY;

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

        #region Skill Selection Variables

        private Dictionary<string, int> _skillSelectionIndexesDict = new();

        private const string _f1_SkillSelectionActionName = "F1_Keyboard Click";
        private const string _f2_SkillSelectionActionName = "F2_Keyboard Click";
        private const string _f3_SkillSelectionActionName = "F3_Keyboard Click";
        private const string _f4_SkillSelectionActionName = "F4_Keyboard Click";
        private const string _f5_SkillSelectionActionName = "F5_Keyboard Click";
        private const string _f6_SkillSelectionActionName = "F6_Keyboard Click";
        private const string _f7_SkillSelectionActionName = "F7_Keyboard Click";
        private const string _f8_SkillSelectionActionName = "F8_Keyboard Click";
        private const string _f9_SkillSelectionActionName = "F9_Keyboard Click";

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            CacheNulls();
            InitModels();
            InitQuickItemsIndexesDict();
            InitSkillSelectionIndexesDict();
            SetScreenSize();
        }

        private void OnDisable()
        {
            DeregisterEvents();
        }

        private void Update()
        {
            SetCurserIcon();
            SetPickedUpLootPosition();
            SetLootInfoPosition();
        }

        #endregion

        #region Initialization

        public void InitUiManager(IPlayerInputReader inputReader)
        {
            _inputReader = inputReader;
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

            _dialogModel = new DialogModel();
            _dialogView.SetModel(_dialogModel);

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

        private void InitSkillSelectionIndexesDict()
        {
            _skillSelectionIndexesDict.Add(_f1_SkillSelectionActionName, 0);
            _skillSelectionIndexesDict.Add(_f2_SkillSelectionActionName, 1);
            _skillSelectionIndexesDict.Add(_f3_SkillSelectionActionName, 2);
            _skillSelectionIndexesDict.Add(_f4_SkillSelectionActionName, 3);
            _skillSelectionIndexesDict.Add(_f5_SkillSelectionActionName, 4);
            _skillSelectionIndexesDict.Add(_f6_SkillSelectionActionName, 5);
            _skillSelectionIndexesDict.Add(_f7_SkillSelectionActionName, 6);
            _skillSelectionIndexesDict.Add(_f8_SkillSelectionActionName, 7);
            _skillSelectionIndexesDict.Add(_f9_SkillSelectionActionName, 8);
        }

        private void SetScreenSize()
        {
            _screenSizeX = Screen.width;
            _screenSizeY = Screen.height;
        }

        #endregion

        #region Cursor

        private void SetCurserIcon()
        {
            if(_inputReader.IsCursorOnUI || _curserModel.IsHoldingLoot())
            {
                _curserModel.SetCursorIconState(CursorIconState.UI);
            }
            else if(_inputReader.IsCursorOnGround)
            {
                _curserModel.SetCursorIconState(CursorIconState.Move);
            }
            else if(_inputReader.IsCursorOnEnemy)
            {
                _curserModel.SetCursorIconState(CursorIconState.Attack);
            }
            else if(_inputReader.IsCursorOnItem)
            {
                _curserModel.SetCursorIconState(CursorIconState.PickUp);
            }
            else if(_inputReader.IsCursorOnNPC)
            {
                _curserModel.SetCursorIconState(CursorIconState.NPC);
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
            RegisterDropLootLeftMouseClickEvent(isHoldingLoot);
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

            if(lootModel.LootCategory == LootCategory.Gold)
            {
                return _inventoryModel.TryAutoPlace_Gold(lootModel);
            }

            return _inventoryModel.TryAutoPlace_Loot(lootModel);
        }

        private void DropLootToTheGround(InputAction.CallbackContext context)
        {
            if(!_curserModel.IsHoldingLoot() || _inputReader.IsCursorOnUI)
                return;

            OnDropLootToTheGround?.Invoke(_curserModel.CurrentHeldLootModel);
            _curserModel.DropLoot();
        }

        private void HandleCurrentHoveredTileGrid(LootModel lootModel, Vector2Int tileIndex)
        {
            _curserModel.SetCurrentHoveredLootModel(lootModel, tileIndex);
        }

        private void HandleCurrentHoveredItemsGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            if(!isCursorOn)
            {
                _curserModel.SetCurrentHoveredItemsGridType(ItemsGridType.None);
                return;
            }

            _curserModel.SetCurrentHoveredItemsGridType(itemsGridModel.ItemsGridType);
        }

        private void HandleItemsGridClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            var cursorLootModel = _curserModel.CurrentHeldLootModel;

            if(itemsGridModel == null || (cursorLootModel == null && lootModel == null))
                return;

            if(cursorLootModel != null)
            {
                var tuple = _inventoryModel.TryHandPlaceOnGrid(itemsGridModel, tileIndex, cursorLootModel);

                if(cursorLootModel.LootCategory == LootCategory.Gold)
                {
                    _curserModel.LootInfoTooltipModel.InvokeChanged();
                }

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
            _pickedUpLootTranform.position = _inputReader.CurrentMousePosition;
        }

        private void SetLootInfoPosition()
        {
            var mousePos = _inputReader.CurrentMousePosition;
            var isCursorOutOfScreen = mousePos.x < 0 || mousePos.x > _screenSizeX || mousePos.y < 0 || mousePos.y > _screenSizeY;

            if(isCursorOutOfScreen)
                return;

            var lootInfoX_HalfSize = _lootInfoRect.sizeDelta.x * 0.5f;
            var lootInfoY_HalfSize = _lootInfoRect.sizeDelta.y * 0.5f;
            var mousePosMinusScreenX = Mathf.Abs(mousePos.x - _screenSizeX);
            var mousePosMinusScreenY = Mathf.Abs(mousePos.y - _screenSizeY);
            Vector3 newPos = new Vector3();

            if(mousePosMinusScreenX < lootInfoX_HalfSize || (_screenSizeX - mousePosMinusScreenX) < lootInfoX_HalfSize)
            {
                newPos.x = _lootInfoRect.position.x;
            }
            else
            {
                newPos.x = mousePos.x;
            }

            if(mousePosMinusScreenY < lootInfoY_HalfSize || (_screenSizeY - mousePosMinusScreenY) < lootInfoY_HalfSize)
            {
                newPos.y = _lootInfoRect.position.y;
            }
            else
            {
                newPos.y = mousePos.y;
            }

            _lootInfoRect.position = newPos;
        }

        #endregion

        #region Inventory

        public List<EquipmentData_ScriptableObject> GetPlayerCurrentlyEquippedGearData()
        {
            return _inventoryModel.CurrentlyEquippedGearData;
        }

        public void RemovePotionFromInventory(Vector2Int index, LootType lootType)
        {
            _inventoryModel.RemovePotionFromInventory(index, lootType);
        }

        private void InvokeToggleInventory()
        {
            ToggleInventoryLogic();
        }

        private void InvokeToggleInventory(InputAction.CallbackContext context)
        {
            ToggleInventoryLogic();
        }

        private void ToggleInventoryLogic()
        {
            var toggledState = !_inventoryModel.IsInventoryOpen;
            _inventoryModel.SetIsInventoryOpen(toggledState);

            if(!_inventoryModel.IsInventoryOpen)
            {
                _curserModel.DeactivateInfoTooltip();
            }

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

            if(_curserModel.CurentHoveredLootModel.LootCategory == LootCategory.Potion)
            {
                OnPotionClicked?.Invoke(_curserModel.CurentHoveredLootModel, _curserModel.CurrentHoveredLootModelRootIndex);
            }
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

        #region Set Player Stats

        public void InitPlayerFullStats(IUnit unit, IEquippedGearAddedStats addedStats)
        {
            _statsModel.InitPlayerFullStats(unit, addedStats);
        }

        public void SetPlayerFullUI(IUnit unit, IEquippedGearAddedStats addedStats)
        {
            var stats = unit.Stats as IPlayerUnitStats;

            SetPlayerVitals(unit.CurrentHP, stats.MaxHP, unit.CurrentMP, stats.MaxMP);
            SetPlayerExp(stats.CurrentExp, stats.ExpToLevelUp);
            SetPlayerLevel(stats.Level);
            SetFullPlayerStatsWithEquippedGear(stats, addedStats);
        }

        public void SetPlayerVitalsExpAndLevel(IUnit unit)
        {
            var stats = unit.Stats as IPlayerUnitStats;

            SetPlayerVitals(unit.CurrentHP, stats.MaxHP, unit.CurrentMP, stats.MaxMP);
            SetPlayerExp(stats.CurrentExp, stats.ExpToLevelUp);
            SetPlayerStats(false);
            SetPlayerLevel(stats.Level);
        }

        public void SetPlayerVitals(int currentHP, int maxHP, int currentMP, int maxMP)
        {
            _hudModel.SetHPAndMP(currentHP, maxHP, currentMP, maxMP);
        }

        public void SetPlayerStats(bool isFullUpdate)
        {
            _statsModel.SetPlayerStats(isFullUpdate);
        }

        private void SetPlayerExp(int currentExp, int expToLevelUp)
        {
            _hudModel.SetExp(currentExp, expToLevelUp);
        }

        private void SetPlayerLevel(int level)
        {
            _hudModel.SetLevel(level);
        }

        private void SetFullPlayerStatsWithEquippedGear(IPlayerUnitStats stats, IEquippedGearAddedStats addedStats)
        {
            _statsModel.SetFullPlayerStats();
            _inventoryModel.SetStrengthAndEquippedWeight(stats.Strength, addedStats.EquippedWeight);
        }

        #endregion

        #region HUD

        public void SetPlayerSkills(List<ISkillData> playerSkills)
        {
            _hudModel.SetPlayerSkills(playerSkills);
        }

        public void SetPlayerActiveSkill(SkillType skillType)
        {
            _hudModel.SetActiveSkill(skillType);
        }

        private List<SkillModel> GetPlayerSkillModels(List<ISkillData> playerSkills)
        {
            var skillModels = new List<SkillModel>();

            foreach(var skillData in playerSkills)
            {
                skillModels.Add(new SkillModel(skillData));
            }

            return skillModels;
        }

        private void HandleHudSkillItemClicked(ISkillData skillData)
        {
            OnHudSkillItemClicked?.Invoke(skillData);
        }

        private void HandleKeyboardFKeysClicked(InputAction.CallbackContext context)
        {
            var index = _skillSelectionIndexesDict[context.action.name];
            var skillModel = _hudModel.SkillModels[index];

            if(skillModel == null || skillModel.SkillData == null)
                return;

            OnHudSkillItemClicked?.Invoke(skillModel.SkillData);
        }

        #endregion

        #region UI Screen Cover

        private void HandleUiScreenCover()
        {
            if(_inventoryModel.IsInventoryOpen && _statsModel.IsPanelOpen)
            {
                OnUIScreenCoverChange?.Invoke(UIScreenCover.FullCover);
                _hudModel.SetIsCloseButtonActive(true);
                _hudModel.SetSkillsBarPosition(SkillsBarPosition.None);

            }
            else if(_inventoryModel.IsInventoryOpen && !_statsModel.IsPanelOpen)
            {
                OnUIScreenCoverChange?.Invoke(UIScreenCover.RightIsCovered);
                _hudModel.SetIsCloseButtonActive(true);
                _hudModel.SetSkillsBarPosition(SkillsBarPosition.Left);
            }
            else if(!_inventoryModel.IsInventoryOpen && _statsModel.IsPanelOpen)
            {
                OnUIScreenCoverChange?.Invoke(UIScreenCover.LeftIsCovered);
                _hudModel.SetIsCloseButtonActive(true);
                _hudModel.SetSkillsBarPosition(SkillsBarPosition.Right);
            }
            else
            {
                OnUIScreenCoverChange?.Invoke(UIScreenCover.NoCover);
                _hudModel.SetIsCloseButtonActive(false);
                _hudModel.SetSkillsBarPosition(SkillsBarPosition.Middle);
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

        #region Dialog

        public void HandleStartConversation(NpcView npcView)
        {
            var speechBubblePosition = GetBubblePosition(npcView);
            _dialogModel.SetSpeechBubblePosition(speechBubblePosition);
            _dialogModel.SetIsBubbleActive(true);
        }

        private Vector3 GetBubblePosition(NpcView npcView)
        {
            var npcPos = npcView.transform.position;
            var screenPoint = Camera.main.WorldToScreenPoint(npcPos);
            var bubbleOffset = npcView.SpeechBubbleOffset;
            return new Vector3(screenPoint.x, screenPoint.y + bubbleOffset, screenPoint.z);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            RegisterInputManagerEvents(true);
            RegisterCursorEvents(true);
            RegisterDialogEvents(true);
            RegisterHudEvents(true);
            RegisterInventoryEvents(true);
            RegisterStatsEvents(true);
            RegisterLevelUpEvents(true);
        }

        private void DeregisterEvents()
        {
            RegisterInputManagerEvents(false);
            RegisterCursorEvents(false);
            RegisterDialogEvents(false);
            RegisterHudEvents(false);
            RegisterInventoryEvents(false);
            RegisterStatsEvents(false);
            RegisterLevelUpEvents(false);
        }

        private void RegisterInputManagerEvents(bool isRegister)
        {
            if(isRegister)
            {
                // Mouse
                _inputReader.ResigterToMouseInputAction(PlayerMouseInputType.RightMouse, HandleMouseRightClickOnLoot);

                // Keyboard Letters
                _inputReader.ResigterToKeyboardLettersInputAction(PlayerKeyboardLettersInputType.I_Keyboard, InvokeToggleInventory);
                _inputReader.ResigterToKeyboardLettersInputAction(PlayerKeyboardLettersInputType.S_Keyboard, ToggleStats);

                // Keyboard Nums
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumOne, HandleKeyboardNumClicked);
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumTwo, HandleKeyboardNumClicked);
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumThree, HandleKeyboardNumClicked);
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFour, HandleKeyboardNumClicked);
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFive, HandleKeyboardNumClicked);
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSix, HandleKeyboardNumClicked);
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSeven, HandleKeyboardNumClicked);
                _inputReader.ResigterToKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumEight, HandleKeyboardNumClicked);

                // Keyboard F Keys
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F1, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F2, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F3, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F4, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F5, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F6, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F7, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F8, HandleKeyboardFKeysClicked);
                _inputReader.ResigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F9, HandleKeyboardFKeysClicked);
            }
            else
            {
                // Mouse
                _inputReader.DeresigterFromMouseInputAction(PlayerMouseInputType.RightMouse, HandleMouseRightClickOnLoot);

                // Keyboard Letters
                _inputReader.DeresigterFromKeyboardLettersInputAction(PlayerKeyboardLettersInputType.I_Keyboard, InvokeToggleInventory);
                _inputReader.DeresigterFromKeyboardLettersInputAction(PlayerKeyboardLettersInputType.S_Keyboard, ToggleStats);

                // Keyboard Nums
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumOne, HandleKeyboardNumClicked);
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumTwo, HandleKeyboardNumClicked);
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumThree, HandleKeyboardNumClicked);
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFour, HandleKeyboardNumClicked);
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumFive, HandleKeyboardNumClicked);
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSix, HandleKeyboardNumClicked);
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumSeven, HandleKeyboardNumClicked);
                _inputReader.DeresigterFromKeyboardNumsInputAction(PlayerKeyboardNumsInputType.NumEight, HandleKeyboardNumClicked);

                // Keyboard F Keys
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F1, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F2, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F3, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F4, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F5, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F6, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F7, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F8, HandleKeyboardFKeysClicked);
                _inputReader.DeresigterToKeyboardFKeysInputAction(PlayerKeyboardFKeysInputType.F9, HandleKeyboardFKeysClicked);
            }
        }

        private void RegisterCursorEvents(bool isRegister)
        {
            if(isRegister)
                _curserView.OnCurserHoldingLootChange += HandleIsCurserHoldingLoot;
            else
                _curserView.OnCurserHoldingLootChange -= HandleIsCurserHoldingLoot;
        }

        private void RegisterDialogEvents(bool isRegister)
        {
            if(isRegister)
            {
                _dialogView.OnCurserEnterUI += CursorEnteredUI;
                _dialogView.OnCurserLeftUI += CursorLeftUI;
            }
            else
            {
                _dialogView.OnCurserEnterUI -= CursorEnteredUI;
                _dialogView.OnCurserLeftUI -= CursorLeftUI;
            }
        }

        private void RegisterHudEvents(bool isRegister)
        {
            var skillUIViews = _hudView.GetSkillUIViews();

            if(isRegister)
            {
                _hudView.OnCurserEnterUI += CursorEnteredUI;
                _hudView.OnCurserLeftUI += CursorLeftUI;
                _hudView.OnInventoryButtonClicked += InvokeToggleInventory;
                _hudView.OnStatsClicked += ToggleStats;
                _hudView.OnSkillItemClicked += HandleHudSkillItemClicked;

                foreach(var view in skillUIViews)
                {
                    view.OnCurserEnterUI += CursorEnteredUI;
                    view.OnCurserLeftUI += CursorLeftUI;
                }
            }
            else
            {
                _hudView.OnCurserEnterUI -= CursorEnteredUI;
                _hudView.OnCurserLeftUI -= CursorLeftUI;
                _hudView.OnInventoryButtonClicked -= InvokeToggleInventory;
                _hudView.OnStatsClicked -= ToggleStats;
                _hudView.OnSkillItemClicked += HandleHudSkillItemClicked;

                foreach(var view in skillUIViews)
                {
                    view.OnCurserEnterUI -= CursorEnteredUI;
                    view.OnCurserLeftUI -= CursorLeftUI;
                }
            }
        }

        private void RegisterInventoryEvents(bool isRegister)
        {
            if(isRegister)
            {
                _inventoryView.OnCurserEnterUI += CursorEnteredUI;
                _inventoryView.OnCurserLeftUI += CursorLeftUI;
                _inventoryView.OnCursorChangedHoverOverGrid += HandleCurrentHoveredItemsGrid;
                _inventoryView.OnTileHovered += HandleCurrentHoveredTileGrid;
                _inventoryView.OnTileClicked += HandleItemsGridClicked;
            }
            else
            {
                _inventoryView.OnCurserEnterUI -= CursorEnteredUI;
                _inventoryView.OnCurserLeftUI -= CursorLeftUI;
                _inventoryView.OnCursorChangedHoverOverGrid -= HandleCurrentHoveredItemsGrid;
                _inventoryView.OnTileHovered -= HandleCurrentHoveredTileGrid;
                _inventoryView.OnTileClicked -= HandleItemsGridClicked;
            }
        }

        private void RegisterStatsEvents(bool isRegister)
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

        private void RegisterLevelUpEvents(bool isRegister)
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

        private void RegisterDropLootLeftMouseClickEvent(bool isRegister) // Registering and Deregistering based on Curser.OnHoldingLootChanged
        {
            if(isRegister)
                _inputReader.ResigterToMouseInputAction(PlayerMouseInputType.LeftMouse, DropLootToTheGround);
            else
                _inputReader.DeresigterFromMouseInputAction(PlayerMouseInputType.LeftMouse, DropLootToTheGround);
        }

        #endregion
    }
}

