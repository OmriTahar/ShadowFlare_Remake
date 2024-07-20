using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.GameManager.Units;
using ShadowFlareRemake.Loot;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private UIController _uiController;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private RewardsManager _rewardsManager;
        [SerializeField] private LootManager _lootManager;

        [Header("Enemies")]
        [SerializeField] private Transform _enemiesParent;

        [Header("Player")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerUnitStats _playerUnitStats;

        [Header("--------------- TESTS: Enemies ---------------")]
        [SerializeField] private EnemyToSpawn _testEnemyToSpawn;
        [SerializeField] private bool _isEnemyActiveOnSpawn;

        [Header("--------------- TESTS: Player ---------------")]
        [SerializeField] private int _healOrDamageAmount = 10;

        private Dictionary<EnemyController, Unit> _enemyUnitsDict = new();
        private Dictionary<Collider, EnemyModel> _enemiesCollidersDict = new();

        private Unit _playerUnit;
        private EquippedGearAddedStats _playerEquippedGearAddedStats = new();
        private GameObject _lastHighlighted_GameObject;
        private HighlightableObject _lastHighlightable;
        private LootView _lastPickedUpLootView;

        private const string _highlightableTag = "Highlightable";
        private const int _lootDropHelper = 3;

        #region Unity Callbacks

        private async void Start()
        {
            await _inputManager.WaitForInitFinish();

            InitEnemies();
            InitPlayer();
            InitUiController();
            RegisterEvents();

            _lootManager.HandleTestSpawnLoot();
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

        private void InitUiController()
        {
            _uiController.InitUiController(_inputManager);
            _uiController.UpdatePlayerFullUI(_playerUnit, _playerEquippedGearAddedStats); // Should handle this when implementing loading system
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _playerController.OnIGotHit += HandlePlayerGotHit;
            _playerController.OnPickedLoot += HandlePlayerPickUpLootFromTheGround;

            _uiController.OnDropLootToTheGround += HandlePlayerDropLootToTheGround;
            _uiController.OnPotionClicked += HandlePlayerUsedQuickItem;
            _uiController.OnIsPlayerHoldingLootChanged += HandlePlayerHoldingLoot;
            _uiController.OnIsCurserOnUiChanged += HandleIsCurserOnUI;
            _uiController.OnPlayerGearChanged += HandlePlayerEquippedGearStats;
        }

        private void DergisterEvents()
        {
            _playerController.OnIGotHit -= HandlePlayerGotHit;
            _playerController.OnPickedLoot -= HandlePlayerPickUpLootFromTheGround;

            _uiController.OnDropLootToTheGround -= HandlePlayerDropLootToTheGround;
            _uiController.OnPotionClicked -= HandlePlayerUsedQuickItem;
            _uiController.OnIsPlayerHoldingLootChanged -= HandlePlayerHoldingLoot;
            _uiController.OnIsCurserOnUiChanged -= HandleIsCurserOnUI;
            _uiController.OnPlayerGearChanged -= HandlePlayerEquippedGearStats;

            foreach(var enemy in _enemyUnitsDict.Keys)
            {
                enemy.OnIGotHit -= HandleEnemyGotHit;
                enemy.OnDeath -= HandleEnemyDied;
            }
        }

        #endregion

        #region Highlightable Objects

        private void HandleHighlightObjectOnCursorFocus()
        {
            var hitCollider = _inputManager.CurrentRaycastHitCollider;

            if(IsValidHighlightableObject(hitCollider))
            {
                HighlightObject(hitCollider);
                return;
            }

            if(_lastHighlightable != null)
                _lastHighlightable.SetIsHighlighted(false);
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
                _lastHighlightable.SetIsHighlighted(false);

            var newHighlightable = newObject.GetComponent<HighlightableObject>();

            if(!newHighlightable.IsHighlightable)
                return;

            newHighlightable.SetIsHighlighted(true);

            _lastHighlighted_GameObject = newObject;
            _lastHighlightable = newHighlightable;
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
            var enemyUnit = new Unit(enemyUnitStats);

            var enemyModel = enemyController.InitEnemy(enemyUnit, _playerController.transform, isEnemyActive);
            _enemyUnitsDict.Add(enemyController, enemyUnit);

            var enemyCollider = enemyController.GetEnemyCollider();
            _enemiesCollidersDict.Add(enemyCollider, enemyModel);

            RegisterEnemyEvents(enemyController);

            if(destroyEnemyToSpawn)
            {
                Destroy(enemyToSpawn.gameObject);
            }
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
            var unit = _enemyUnitsDict[enemyController];

            var isCritialHit = CombatLogic.HandleTakeDamageAndReturnIsCritialHit(attack, unit);

            enemyController.SetEnemyUnitAfterHit(unit, isCritialHit);
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
                _uiController.ShowLevelUpPopup(_playerUnitStats.Level, levelUpReward);
            }

            _uiController.UpdatePlayerVitalsExpAndLevel(_playerUnit);
            _lootManager.HandleLootDrop(enemyStats.Level, enemyStats.LootDropChance, enemyPosition);
        }

        #endregion

        #region Player

        private void InitPlayer()
        {
            _playerUnit = new Unit(_playerUnitStats);
            _playerController.InitPlayer(_playerUnit, _inputManager);
        }

        private void HandlePlayerGotHit(Attack attack)
        {
            var isCritialHit = CombatLogic.HandleTakeDamageAndReturnIsCritialHit(attack, _playerUnit);
            _playerController.SetIsLastHitWasCritialHit(isCritialHit);
            _uiController.UpdatePlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
        }

        private void HandlePlayerPickUpLootFromTheGround(Collider lootCollider)
        {
            _lastPickedUpLootView = lootCollider.GetComponent<LootView>();
            _lastPickedUpLootView.gameObject.SetActive(false);
            var lootModel = _lastPickedUpLootView.GetLootModel();

            if(!_uiController.TryPickUpLootFromTheGround(lootModel))
            {
                _lastPickedUpLootView.gameObject.SetActive(true);
                lootModel.InvokeDropAnimation();
                return;
            }

            if(lootModel.LootCategory == Enums.LootCategory.Equipment)
            {
                HandlePlayerEquippedGearStats(_uiController.GetPlayerCurrentlyEquippedGearData());
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
            var hpHealAmount = potionData.HpRestoreAmount;
            var mpHealAmount = potionData.MpRestoreAmount;
            bool hasHealed = false;

            if(!_playerUnit.IsHpFull() && hpHealAmount > 0)
            {
                _playerUnit.HealHP(hpHealAmount);
                hasHealed = true;
            }

            if(!_playerUnit.IsMpFull() && mpHealAmount > 0)
            {
                _playerUnit.HealMP(mpHealAmount);
                hasHealed = true;
            }

            if(!hasHealed)
                return;

            _uiController.UpdatePlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
            _uiController.RemovePotionFromInventory(index, lootModel.LootData.LootType);
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
            _uiController.UpdatePlayerFullUI(_playerUnit, _playerEquippedGearAddedStats);
        }

        #endregion

        #region Loot

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

        #region Tests

        public void TestSpawnEnemy()
        {
            _testEnemyToSpawn.SetGameobjectNameAsEnemy();
            InitEnemyLogic(_testEnemyToSpawn, _isEnemyActiveOnSpawn, false);
        }

        public void TestHitPlayer()
        {
            _playerUnit.TakeDamage(_healOrDamageAmount);
            _playerController.SetIsLastHitWasCritialHit(true);
            _uiController.UpdatePlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
        }

        public void TestHealPlayer()
        {
            _playerUnit.HealHP(_healOrDamageAmount);
            _uiController.UpdatePlayerVitals(_playerUnit.CurrentHP, _playerUnitStats.MaxHP, _playerUnit.CurrentMP, _playerUnitStats.MaxMP);
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

                if(GUILayout.Button("Hit Player"))
                {
                    gameManager.TestHitPlayer();
                }

                GUILayout.Space(15);

                if(GUILayout.Button("Heal Player"))
                {
                    gameManager.TestHealPlayer();
                }

                GUILayout.Space(10);
            }
        }

#endif

        #endregion
    }
}

