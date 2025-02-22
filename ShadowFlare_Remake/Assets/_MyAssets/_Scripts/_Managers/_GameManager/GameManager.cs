using ShadowFlareRemake.CamerasManagement;
using ShadowFlareRemake.Combat;
using ShadowFlareRemake.CombatManagement;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.EnemiesRestrictedData;
using ShadowFlareRemake.GameManagerRestrictedData;
using ShadowFlareRemake.InputManagement;
using ShadowFlareRemake.Interactables;
using ShadowFlareRemake.Loot;
using ShadowFlareRemake.LootManagement;
using ShadowFlareRemake.Npc;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.PlayerRestrictedData;
using ShadowFlareRemake.RewardsManagement;
using ShadowFlareRemake.Skills;
using ShadowFlareRemake.UI;
using ShadowFlareRemake.UI.Highlightables;
using ShadowFlareRemake.UIManagement;
using ShadowFlareRemake.UnitsRestrictedData;
using ShadowFlareRemake.VFX;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private CombatManager _combatManager;
        [SerializeField] private RewardsManager _rewardsManager;
        [SerializeField] private LootManager _lootManager;
        [SerializeField] private CamerasManager _camerasManager;

        [Header("Player")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerUnitStats _playerUnitStatsToCopy;
        [SerializeField] private VisualEffectsSubView _playerVfxView;

        [Header("Enemies")]
        [SerializeField] private Transform _enemiesParent;

        [Header("Loading")]
        [SerializeField] private Animator _loadingScreenAnimator;
        [SerializeField] private bool _showLoadingScreen;

        [Header("--------------- Test ---------------")]
        [SerializeField] private EnemyToSpawn _testEnemyToSpawn;
        [SerializeField] private bool _isEnemyActiveOnSpawn;
        [SerializeField] private int _playerRestoreOrReduceVitalsAmount = 10;

        private Unit _playerUnit;
        private PlayerModel _playerModel;
        private PlayerUnitStats _playerUnitStats;
        private EquippedGearAddedStats _playerEquippedGearAddedStats = new();

        private Dictionary<EnemyController, EnemyConcreteData> _enemiesDict = new();
        private GameObject _lastHighlighted_GameObject;
        private HighlightableBehaviour _lastHighlightable;
        private NpcDataContainer _lastNpc = new();
        private Interactable _lastInteractable;
        private LootView _lastPickedUpLootView;

        private readonly int _LoadingFadeOutAnimHash = Animator.StringToHash("FadeOut");
        private readonly int _LoadingFadeInAnimHash = Animator.StringToHash("FadeIn");
        private const string _highlightableTag = "Highlightable";
        private const int _lootDropHelper = 3;

        #region MonoBehaviour

        private async void Start()
        {
            var startTime = PrintAndGetLoadingStartTime();
            _loadingScreenAnimator.gameObject.SetActive(_showLoadingScreen);

            await _inputManager.WaitForInitFinish();
            HandleInitializtion();
            RegisterEvents();
            HandleStartingLoot();
            _lootManager.HandleTestSpawnLoot();

            if(_showLoadingScreen)
                _loadingScreenAnimator.SetTrigger(_LoadingFadeOutAnimHash);

            PrintLoadingEndTime(startTime);
        }

        private void Update()
        {
            HandleHighlightObjectOnCursorFocus();
        }

        private void OnDestroy()
        {
            DergisterEvents();
        }

        #endregion

        #region Initialization

        private float PrintAndGetLoadingStartTime()
        {
            float startTime = Time.realtimeSinceStartup;
            print("Game Manager: Loading Start Time: " + startTime);
            return startTime;
        }

        private void PrintLoadingEndTime(float startTime)
        {
            float endTime = Time.realtimeSinceStartup;
            float loadTime = endTime - startTime;
            print("Game Manager: Loading End Time: " + endTime);
            print("Game Manager: Load Time: " + loadTime);
        }

        private void HandleInitializtion()
        {
            InitEnemies();
            InitPlayer();
            InitUiManager();
        }

        private void InitUiManager()
        {
            _uiManager.InitUiManager(_inputManager);
            _uiManager.InitPlayerFullStats(_playerUnit, _playerEquippedGearAddedStats);
            _uiManager.SetPlayerFullUI(_playerUnit, _playerEquippedGearAddedStats);
            _uiManager.SetPlayerSkills(GetPlayerSkills());
            _uiManager.SetPlayerActiveSkill(SkillType.MeleeAttack);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            RegisterPlayerControllerEvents(true);
            RegisterUIEvents(true);
        }

        private void DergisterEvents()
        {
            RegisterPlayerControllerEvents(false);
            RegisterUIEvents(false);
            DeregisterFromEnemiesEvents();
        }

        private void RegisterPlayerControllerEvents(bool isRegister)
        {
            if(isRegister)
            {
                _playerController.OnIGotHit += HandlePlayerGotHit;
                _playerController.OnPickedLoot += HandlePlayerPickUpLootFromTheGround;
                _playerController.OnPlayerAttack += HandlePlayerAttack;
                _playerController.OnClickedOnNpc += HandlePlayerClickedOnNpc;
                _playerController.OnTalkingToNpc += HandlePlayerTalkingToNpc;
                _playerController.OnFinishTalkingToNpc += HandlePlayerFinishedTalkingToNpc;
                _playerController.OnClickedOnInteractable += HandlePlayerClickedOnInteractable;
                _playerController.OnInteractingWithInteractable += HandlePlayerInteractingWithInteractable;
                _playerController.OnFinishedInteractingWithInteractable += HandlePlayerFinishedInteracation;
            }
            else
            {
                _playerController.OnIGotHit -= HandlePlayerGotHit;
                _playerController.OnPickedLoot -= HandlePlayerPickUpLootFromTheGround;
                _playerController.OnPlayerAttack -= HandlePlayerAttack;
                _playerController.OnClickedOnNpc -= HandlePlayerClickedOnNpc;
                _playerController.OnTalkingToNpc -= HandlePlayerTalkingToNpc;
                _playerController.OnFinishTalkingToNpc -= HandlePlayerFinishedTalkingToNpc;
                _playerController.OnClickedOnInteractable -= HandlePlayerClickedOnInteractable;
                _playerController.OnInteractingWithInteractable -= HandlePlayerInteractingWithInteractable;
                _playerController.OnFinishedInteractingWithInteractable -= HandlePlayerFinishedInteracation;
            }
        }

        private void RegisterUIEvents(bool isRegister)
        {
            if(isRegister)
            {
                _uiManager.OnPlayerGearChanged += HandlePlayerEquippedGearStats;
                _uiManager.OnIsCurserOnUiChanged += HandleIsCurserOnUI;
                _uiManager.OnIsPlayerHoldingLootChanged += HandlePlayerHoldingLoot;
                _uiManager.OnDropLootToTheGround += HandlePlayerDropLootToTheGround;
                _uiManager.OnPotionClicked += HandlePlayerUsedQuickItem;
                _uiManager.OnUIScreenCoverChange += HandleCamerasOnUiCoverChange;
                _uiManager.OnHudSkillItemClicked += HandleHudSkillItemClicked;
                _uiManager.OnNpcHealedPlayer += HandleNpcHealedPlayer;
                _uiManager.OnFinishedDialog += HandlePlayerFinishedTalkingToNpc;
            }
            else
            {
                _uiManager.OnPlayerGearChanged -= HandlePlayerEquippedGearStats;
                _uiManager.OnIsCurserOnUiChanged -= HandleIsCurserOnUI;
                _uiManager.OnIsPlayerHoldingLootChanged -= HandlePlayerHoldingLoot;
                _uiManager.OnDropLootToTheGround -= HandlePlayerDropLootToTheGround;
                _uiManager.OnPotionClicked -= HandlePlayerUsedQuickItem;
                _uiManager.OnUIScreenCoverChange -= HandleCamerasOnUiCoverChange;
                _uiManager.OnHudSkillItemClicked -= HandleHudSkillItemClicked;
                _uiManager.OnNpcHealedPlayer -= HandleNpcHealedPlayer;
                _uiManager.OnFinishedDialog -= HandlePlayerFinishedTalkingToNpc;
            }
        }

        private void DeregisterFromEnemiesEvents()
        {
            foreach(var enemy in _enemiesDict.Keys)
            {
                enemy.OnIGotHit -= HandleEnemyGotHit;
                enemy.OnDeath -= HandleEnemyDied;
            }
        }

        #endregion

        #region Highlightables

        private void HandleHighlightObjectOnCursorFocus()
        {
            if(_inputManager.IsCursorOnUI)
            {
                if(_lastHighlightable != null)
                    SetIsHighlighted(false);

                return;
            }

            var hitCollider = _inputManager.CurrentRaycastHitCollider;

            if(IsValidHighlightableObject(hitCollider))
            {
                HighlightObject(hitCollider);
                HandleCacheLatestEntityType();
                return;
            }

            if(_lastHighlightable != null)
                SetIsHighlighted(false);
        }

        private void HandleCacheLatestEntityType()
        {

            switch(_lastHighlightable.EntityType)
            {
                case EntityType.Interactable:
                    CacheLastInteractable();
                    break;

                case EntityType.Npc:
                    CacheLastNpc();
                    break;

                default:
                    break;
            }
        }

        private bool IsValidHighlightableObject(Collider hitCollider)
        {
            return (hitCollider != null && hitCollider.gameObject.tag == _highlightableTag);
        }

        private void HighlightObject(Collider hitCollider)
        {
            var newObject = hitCollider.gameObject;

            if(newObject == _lastHighlighted_GameObject && _lastHighlightable.IsHighlighted)
                return;

            if(_lastHighlightable != null)
            {
                SetIsHighlighted(false);
            }

            var newHighlightable = newObject.GetComponent<HighlightableBehaviour>();

            if(!newHighlightable.IsHighlightable)
                return;

            _lastHighlighted_GameObject = newObject;
            _lastHighlightable = newHighlightable;
            SetHighlightableData();
            SetIsHighlighted(true);
        }

        private void SetIsHighlighted(bool isHighlighted)
        {
            _lastHighlightable.HandleSetIsHighlighted(isHighlighted);
            _uiManager.SetIsHighlightableNameActive(isHighlighted);

            if(!isHighlighted)
                _uiManager.SetHighlightableEntityTransform(null);
        }

        private void SetHighlightableData()
        {
            var highlightableData = _lastHighlightable.GetHighlightableData();
            _uiManager.SetHighlightableData(highlightableData, _lastHighlighted_GameObject.transform);
        }

        private void CacheLastNpc()
        {
            var npc = _lastHighlightable.GetNpcBehaviour();

            if(npc == null)
                return;

            _lastNpc.HighlightableBehaviour = _lastHighlightable;
            _lastNpc.Npc = npc;
        }

        #endregion

        #region Input Manager

        private void HandlePlayerHoldingLoot(bool isHoldingLoot)
        {
            _inputManager.SetIsHoldingLoot(isHoldingLoot);
        }

        private void HandleIsCurserOnUI(bool isCurserOnUI)
        {
            _inputManager.SetIsCursorOnUI(isCurserOnUI);
        }

        #endregion

        #region UI

        private void HandleHudSkillItemClicked(ISkillData activeSkill)
        {
            _playerModel.SetActiveSkill(activeSkill);
            _uiManager.SetPlayerActiveSkill(activeSkill.SkillType);
        }

        #endregion

        #region Cameras

        private void HandleCamerasOnUiCoverChange(UIScreenCover uIScreenCover)
        {
            _camerasManager.SetActiveCamera(uIScreenCover);
        }

        #endregion

        #region Enemies

        private void InitEnemies()
        {
            var enemiesToSpawn = _enemiesParent.GetComponentsInChildren<EnemyToSpawn>();

            foreach(var enemyToSpawn in enemiesToSpawn)
            {
                InitEnemyLogic(enemyToSpawn, _isEnemyActiveOnSpawn);
            }
        }

        private void InitEnemyLogic(EnemyToSpawn enemyToSpawn, bool isEnemyActive, bool destroyEnemyToSpawn = true)
        {
            if(!IsValidEnemyToSpawn(enemyToSpawn))
                return;

            var spawnPoint = enemyToSpawn.transform;
            var enemyController = Instantiate(enemyToSpawn.EnemyPrefab, spawnPoint.position, spawnPoint.rotation, _enemiesParent);
            var enemyUnitStats = enemyToSpawn.EnemyUnit;
            var enemySkills = GetEnemySkills(enemyUnitStats);
            var enemyUnit = new Unit(enemyUnitStats, enemySkills);
            var enemyModel = new EnemyModel(enemyUnit);
            var enemyConcreteData = new EnemyConcreteData(enemyUnit, enemyModel);

            enemyController.InitEnemy(enemyModel, _playerController.transform, isEnemyActive);
            _enemiesDict.Add(enemyController, enemyConcreteData);
            RegisterEnemyEvents(enemyController);

            if(destroyEnemyToSpawn)
                Destroy(enemyToSpawn.gameObject);
        }

        private List<ISkillData> GetEnemySkills(EnemyUnitStats enemyUnitStats)
        {
            var skillsList = new List<ISkillData>();

            foreach(var skillData in enemyUnitStats.Skills)
            {
                skillsList.Add(skillData);
            }

            return skillsList;
        }

        private bool IsValidEnemyToSpawn(EnemyToSpawn enemyToSpawn)
        {
            if(enemyToSpawn == null || enemyToSpawn.EnemyPrefab == null || enemyToSpawn.EnemyUnit == null || !enemyToSpawn.isActiveAndEnabled)
            {
                return false;
            }

            return true;
        }

        private void RegisterEnemyEvents(EnemyController enemyController)
        {
            enemyController.OnIGotHit += HandleEnemyGotHit;
            enemyController.OnDeath += HandleEnemyDied;
        }

        private void HandleEnemyGotHit(Attack attack, EnemyController enemyController)
        {
            var enemyData = _enemiesDict[enemyController];

            var receivedAttackData = _combatManager.GetReceivedAttackData(attack, enemyData.EnemyUnit.Stats);

            if(receivedAttackData.InflictedDamage <= 0)
                return;

            enemyData.EnemyUnit.ReduceHP(receivedAttackData.InflictedDamage);
            enemyData.EnemyModel.SetIsReceivedCritialHit(receivedAttackData.IsCritialHit);

            SetHighlightableData();
        }

        private void HandleEnemyDied(IEnemyUnitStats enemyStats, Vector3 enemyPosition)
        {
            var expReward = _rewardsManager.GetExpReward(_playerUnitStats, enemyStats);
            _playerUnitStats.GiveExpReward(expReward);

            if(expReward.IsPendingLevelUp)
            {
                var levelUpReward = _rewardsManager.GetLevelUpReward(_playerUnitStats);
                _playerUnitStats.GiveLevelUpReward(levelUpReward);
                _playerUnit.FullHeal();
                _uiManager.ShowLevelUpPopup(_playerUnitStats.Level, levelUpReward);
            }

            _uiManager.SetPlayerVitalsExpAndLevel(_playerUnit);
            _uiManager.SetPlayerStats(expReward.IsPendingLevelUp);

            _lootManager.HandleLootDrop(enemyStats.Level, enemyStats.LootDropChance, enemyPosition);
        }

        #endregion

        #region Player

        private void InitPlayer()
        {
            _playerUnitStats = ScriptableObject.CreateInstance<PlayerUnitStats>();
            _playerUnitStats.SetStatsFromCopy(_playerUnitStatsToCopy);
            var playerSkills = GetPlayerSkills();

            _playerUnit = new Unit(_playerUnitStats, playerSkills);
            _playerModel = new PlayerModel(_playerUnit);

            var meleeSkill = GetSkillDataFromSkillType(playerSkills, SkillType.MeleeAttack);
            _playerModel.SetActiveSkill(meleeSkill);

            _playerController.InitPlayer(_playerModel, _inputManager);
        }

        private void HandlePlayerGotHit(Attack attack)
        {
            var receivedAttackData = _combatManager.GetReceivedAttackData(attack, _playerUnitStats);

            if(receivedAttackData.InflictedDamage <= 0)
                return;

            _playerUnit.ReduceHP(receivedAttackData.InflictedDamage);
            _playerModel.SetIsLastHitWasCritialHit(receivedAttackData.IsCritialHit);

            _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
        }

        private void HandlePlayerPickUpLootFromTheGround(Collider lootCollider)
        {
            _lastPickedUpLootView = lootCollider.GetComponent<LootView>();
            _lastPickedUpLootView.gameObject.SetActive(false);
            var lootModel = _lastPickedUpLootView.GetLootModel();

            if(!_uiManager.TryPickUpLootFromTheGround(lootModel))
            {
                _lastPickedUpLootView.gameObject.SetActive(true);
                lootModel.InvokeDropAnimation();
                return;
            }

            if(lootModel.LootCategory == LootCategory.Equipment)
            {
                HandlePlayerEquippedGearStats(_uiManager.GetPlayerCurrentlyEquippedGearData());
            }
        }

        private void HandlePlayerDropLootToTheGround(LootModel lootModel)
        {
            var worldLoot = _lootManager.InstantiateLootPrefab();
            worldLoot.transform.position = GetPlayerDroppingLootPos();

            var lootView = worldLoot.GetComponentInChildren<LootView>();
            lootView.SetModel(lootModel);
            lootModel.InvokeDropAnimation();

            HandlePlayerHoldingLoot(false);
            _playerController.SetIsLastActionWasMove(false);
        }

        private void HandlePlayerUsedQuickItem(LootModel lootModel, Vector2Int index)
        {
            var potionData = lootModel.LootData as PotionData_ScriptableObject;
            var hpRestoreAmount = potionData.HpRestoreAmount;
            var mpRestoreAmount = potionData.MpRestoreAmount;
            bool hasRestored = false;

            if(!_playerUnit.IsHpFull() && hpRestoreAmount > 0)
            {
                _playerUnit.RestoreHP(hpRestoreAmount);
                hasRestored = true;
            }

            if(!_playerUnit.IsMpFull() && mpRestoreAmount > 0)
            {
                _playerUnit.RestoreMP(mpRestoreAmount);
                hasRestored = true;
            }

            if(!hasRestored)
                return;

            _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
            _uiManager.RemovePotionFromInventory(index, lootModel.LootData.LootType);
        }

        private void HandlePlayerEquippedGearStats(List<EquipmentData_ScriptableObject> currentlyEquippedGear)
        {
            _playerUnitStats.RemoveEquippedGearAddedStats(_playerEquippedGearAddedStats);
            _playerEquippedGearAddedStats.ResetValues();

            foreach(var equipmentData in currentlyEquippedGear)
            {
                _playerEquippedGearAddedStats.AddEquippedGearStats(equipmentData);
            }

            _playerUnitStats.SetEquippedGearAddedStats(_playerEquippedGearAddedStats);
            _uiManager.SetPlayerFullUI(_playerUnit, _playerEquippedGearAddedStats);
        }

        private List<ISkillData> GetPlayerSkills()
        {
            var skillsList = new List<ISkillData>();

            foreach(var skillData in _playerUnitStats.Skills)
            {
                skillsList.Add(skillData);
            }

            return skillsList;
        }

        private ISkillData GetSkillDataFromSkillType(List<ISkillData> skills, SkillType skillType)
        {
            foreach(var skill in skills)
            {
                if(skill == null)
                {
                    continue;
                }

                if(skill.SkillType == skillType)
                {
                    return skill;
                }
            }

            return null;
        }

        private void HandlePlayerAttack(bool isUsingSkill)
        {
            if(!isUsingSkill)
            {
                _playerModel.SetAttackState(true, false);
                return;
            }

            var skillMpCost = _playerModel.ActiveSkill.MpCost;
            var hasEnoughMana = skillMpCost <= _playerUnit.CurrentMP;

            if(_playerModel.ActiveSkill != null && hasEnoughMana)
            {
                if(skillMpCost > 0)
                {
                    _playerUnit.ReduceMP(_playerModel.ActiveSkill.MpCost);
                    _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
                }

                _playerModel.SetAttackState(true, true);
            }
        }

        private void PlayHealEffectOnPlayer()
        {
            _playerVfxView.SetIsPlayingEffect(VfxType.Heal, true);
        }

        #endregion

        #region Npc

        private void HandlePlayerClickedOnNpc()
        {
            var currentTalkingNpc = _uiManager.GetCurrentTalkingNpc();

            if(currentTalkingNpc != null && _lastNpc.Npc != currentTalkingNpc)
            {
                HandlePlayerFinishedTalkingToNpc();
            }

            _uiManager.SetCurrentTalkingNpc(_lastNpc.Npc);
        }

        private void HandlePlayerTalkingToNpc()
        {
            var currentTalkingNpc = _uiManager.GetCurrentTalkingNpc();
            currentTalkingNpc.LookAtTransform(_playerController.transform);
            _uiManager.HandleDialog(false);
        }

        private void HandlePlayerFinishedTalkingToNpc()
        {
            _uiManager.HandleFinishDialog();

            if(_lastHighlightable != null)
                _uiManager.SetIsHighlightableNameActive(_lastHighlightable.IsHighlighted);
        }

        private void HandleNpcHealedPlayer(NpcBehaviour npc)
        {
            if(npc == null || _playerUnit.IsVitalsFull())
                return;

            npc.CastHealAnimation();
            _playerUnit.FullHeal();
            _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
            PlayHealEffectOnPlayer();
        }

        #endregion

        #region Interactables

        private void CacheLastInteractable()
        {
            var interactable = _lastHighlightable.GetInteractable();

            if(interactable == null)
                return;

            _lastInteractable = interactable;
        }

        private void HandlePlayerClickedOnInteractable()
        {

        }

        private void HandlePlayerInteractingWithInteractable()
        {
            _lastInteractable.Interact();
            bool isFinished = false;

            switch(_lastInteractable.Type)
            {
                case InteractableType.Warehouse:
                    WarehouseInteraction(isFinished);
                    break;

                case InteractableType.Temple:
                    break;

                default:
                    break;
            }
        }

        private void HandlePlayerFinishedInteracation()
        {
            _lastInteractable.FinishInteraction();

            switch(_lastInteractable.Type)
            {
                case InteractableType.Warehouse:
                    WarehouseInteraction(true);
                    break;

                case InteractableType.Temple:
                    break;

                default:
                    break;
            }
        }

        private void WarehouseInteraction(bool isFinished)
        {
            if(!isFinished)
            {
                _uiManager.ToggleWarehouse(true);
                return;
            }

            _uiManager.ToggleWarehouse(false);
        }

        #endregion

        #region Loot

        private void HandleStartingLoot()
        {
            GivePlayerStartingLoot();
            GiveWarehouseStartingLoot();
        }

        private void GivePlayerStartingLoot()
        {
            var startingLootData = _lootManager.GetPlayerStartingLoot();
            var startingLootModels = GenerateLootModels(startingLootData);

            foreach(var lootModel in startingLootModels)
            {
                _uiManager.TryPickUpLootFromTheGround(lootModel);
            }
        }

        private void GiveWarehouseStartingLoot()
        {
            var startingLootData = _lootManager.GetWarehouseStartingLoot();
            var startingLootModels = GenerateLootModels(startingLootData);
            _uiManager.PlaceStartingLootInWarehouse(startingLootModels);
        }

        private List<LootModel> GenerateLootModels(List<LootData_ScriptableObject> lootData)
        {
            var lootModels = new List<LootModel>();

            foreach(var data in lootData)
            {
                lootModels.Add(new LootModel(data));
            }

            return lootModels;
        }

        private Vector3 GetPlayerDroppingLootPos()
        {
            var playerPos = _playerController.transform.position;
            var rayCastPos = _inputManager.CurrentRaycastHit.point;

            var direction = (rayCastPos - playerPos).normalized;
            var newPos = playerPos + (direction * _lootDropHelper);
            newPos.y = 0;

            return newPos;
        }

        #endregion

        #region Loading Screen



        #endregion

        #region Tests

        public void TestSpawnEnemy()
        {
            _testEnemyToSpawn.SetGameobjectNameAsEnemy();
            InitEnemyLogic(_testEnemyToSpawn, _isEnemyActiveOnSpawn, false);
        }

        public void TestReducePlayerHealth()
        {
            _playerUnit.ReduceHP(_playerRestoreOrReduceVitalsAmount);
            _playerModel.SetIsLastHitWasCritialHit(true);
            _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
        }

        public void TestRestorePlayerHealth()
        {
            _playerUnit.RestoreHP(_playerRestoreOrReduceVitalsAmount);
            _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
        }

        public void TestReducePlayerMana()
        {
            _playerUnit.ReduceMP(_playerRestoreOrReduceVitalsAmount);
            _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
        }

        public void TestRestorePlayerMana()
        {
            _playerUnit.RestoreMP(_playerRestoreOrReduceVitalsAmount);
            _uiManager.SetPlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
        }

#if UNITY_EDITOR

        [UnityEditor.CustomEditor(typeof(GameManager))]
        public class Drawer : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                GUILayout.Space(20);

                var gameManager = target as GameManager;

                if(GUILayout.Button("Spawn Enemy"))
                {
                    gameManager.TestSpawnEnemy();
                }

                GUILayout.Space(15);

                if(GUILayout.Button("Reduce Player Health"))
                {
                    gameManager.TestReducePlayerHealth();
                }

                GUILayout.Space(5);

                if(GUILayout.Button("Restore Player Health"))
                {
                    gameManager.TestRestorePlayerHealth();
                }

                GUILayout.Space(15);

                if(GUILayout.Button("Reduce Player Mana"))
                {
                    gameManager.TestReducePlayerMana();
                }

                GUILayout.Space(5);

                if(GUILayout.Button("Restore Player Mana"))
                {
                    gameManager.TestRestorePlayerMana();
                }

                GUILayout.Space(10);
            }
        }

#endif

        #endregion
    }
}

