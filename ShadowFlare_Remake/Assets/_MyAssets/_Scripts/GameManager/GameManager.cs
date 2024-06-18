using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.GameManager.Units;
using ShadowFlareRemake.Loot;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputManager _inputManager;

        [Header("Rewards")]
        [SerializeField] private RewardsManager _rewardsManager;

        [Header("UI")]
        [SerializeField] private UIController _uiController;

        [Header("Enemies")]
        [SerializeField] private Transform _enemiesParent;

        [Header("Player")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerUnitStats _playerUnitStats;

        [Header("Loot")]
        [SerializeField] private Transform _testLootParent;
        [SerializeField] private GameObject _testLootPrefab;
        [SerializeField] private List<Loot_ScriptableObject> _testLootData;

        private Unit _playerUnit;
        private Dictionary<EnemyController, Unit> _enemyUnitsDict = new();
        private Dictionary<Collider, EnemyModel> _enemiesCollidersDict = new();

        private GameObject _lastHighlighted_GameObject;
        private HighlightableObject _lastHighlightable;

        private LootView _lastPickedUpLootView;

        private const string _highlightableTag = "Highlightable";
        private const int _lootDropHelper = 3;

        #region Unity Callbacks

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            await _inputManager.WaitForInitFinish();

            InitEnemies();
            InitPlayer();
            InitUiController();

            RegisterEvents();

            HandleTestSpawnLoot();
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
            _uiController.UpdatePlayerUI(_playerUnit);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _playerController.OnIGotHit += HandlePlayerGotHit;
            _playerController.OnPickedLoot += HandlePlayerPickUpLootFromTheGround;

            _uiController.OnDropLootToTheGround += HandlePlayerDropLootToTheGround;
            _uiController.OnIsPlayerHoldingLootChanged += HandlePlayerHoldingLoot;
            _uiController.OnIsCurserOnUiChanged += HandleIsCurserOnUI;
        }

        private void DergisterEvents()
        {
            _playerController.OnIGotHit -= HandlePlayerGotHit;
            _playerController.OnPickedLoot -= HandlePlayerPickUpLootFromTheGround;

            _uiController.OnDropLootToTheGround -= HandlePlayerDropLootToTheGround;
            _uiController.OnIsPlayerHoldingLootChanged -= HandlePlayerHoldingLoot;
            _uiController.OnIsCurserOnUiChanged -= HandleIsCurserOnUI;

            foreach(var enemy in _enemyUnitsDict.Keys)
            {
                enemy.OnIGotHit -= HandleEnemyGotHit;
                enemy.OnDeath -= HandleEnemyDied;
            }
        }

        #endregion

        #region General

        private void HandleHighlightObjectOnCursorFocus()
        {
            var hitCollider = _inputManager.CurrentRaycastHitCollider;

            if(hitCollider != null && hitCollider.gameObject.tag == _highlightableTag)
            {
                var newObject = hitCollider.gameObject;

                if(newObject == _lastHighlighted_GameObject && _lastHighlightable.IsHighlighted)
                    return;

                if(_lastHighlightable != null)
                    _lastHighlightable.SetIsHighlighted(false);

                var newHighlightable = newObject.GetComponent<HighlightableObject>();
                newHighlightable.SetIsHighlighted(true);

                _lastHighlighted_GameObject = newObject;
                _lastHighlightable = newHighlightable;
                return;
            }

            if(_lastHighlightable != null)
                _lastHighlightable.SetIsHighlighted(false);
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
                InitEnemyLogic(enemyToSpawn);
            }
        }

        private void InitEnemyLogic(EnemyToSpawn enemyToSpawn)
        {
            if(enemyToSpawn == null)
            {
                Debug.LogError("EnemyToSpawn Null Reference!");
                return;
            }

            var spawnPoint = enemyToSpawn.transform;
            var enemyController = Instantiate(enemyToSpawn.EnemyPrefab, spawnPoint.position, spawnPoint.rotation, _enemiesParent);

            var enemyUnitStats = enemyToSpawn.EnemyUnit;
            var enemyUnit = new Unit(enemyUnitStats);

            var enemyModel = enemyController.InitEnemy(enemyUnit, _playerController.transform);
            _enemyUnitsDict.Add(enemyController, enemyUnit);

            var enemyCollider = enemyController.GetEnemyCollider();
            _enemiesCollidersDict.Add(enemyCollider, enemyModel);

            RegisterEnemyEvents(enemyController);
            Destroy(enemyToSpawn.gameObject);
        }

        private void RegisterEnemyEvents(EnemyController enemyController)
        {
            enemyController.OnIGotHit += HandleEnemyGotHit;
            enemyController.OnDeath += HandleEnemyDied;
        }

        private void HandleEnemyGotHit(Attack attack, EnemyController enemyController)
        {
            var unit = _enemyUnitsDict[enemyController];

            CombatLogic.HandleTakeDamage(attack, unit);

            enemyController.SetEnemyUnitAndUnitHandler(unit);
        }

        private void HandleEnemyDied(IEnemyUnitStats enemyStats)
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

            _uiController.UpdatePlayerUI(_playerUnit);
        }

        #endregion

        #region Player

        private void InitPlayer()
        {
            _playerUnit = new Unit(_playerUnitStats);
            _playerController.InitPlayer(_playerUnit, _inputManager);
        }

        private void HandlePlayerGotHit(Attack attack, IUnitStats unit)
        {

        }

        private void HandlePlayerPickUpLootFromTheGround(Collider lootCollider)
        {
            _lastPickedUpLootView = lootCollider.GetComponent<LootView>();
            var lootModel = _lastPickedUpLootView.GetLootModel();

            if(!_uiController.TryPickUpLootFromTheGround(lootModel))
            {
                lootModel.InvokeDropAnimation();
                return;
            }

            _lastPickedUpLootView.gameObject.SetActive(false);
        }

        private void HandlePlayerDropLootToTheGround(LootModel lootModel)
        {
            var worldLoot = Instantiate(_testLootPrefab);
            worldLoot.transform.position = GetLootDropPos();

            var lootView = worldLoot.GetComponentInChildren<LootView>();
            lootView.SetModel(lootModel);
            lootModel.InvokeDropAnimation();

            HandlePlayerHoldingLoot(false);
            _playerController.SetIsLastActionWasMove(false);
        }

        #endregion

        #region Loot

        private Vector3 GetLootDropPos()
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

        private void HandleTestSpawnLoot()
        {
            float spawnPosX = _testLootParent.transform.position.x;
            foreach(var lootData in _testLootData)
            {
                TestSpawnLoot(lootData, spawnPosX);
                spawnPosX += 1.25f;
            }
        }

        private void TestSpawnLoot(Loot_ScriptableObject lootData, float posX)
        {
            var lootModel = new LootModel(lootData);

            var fuckme = Instantiate(_testLootPrefab, _testLootParent);
            var pos = fuckme.transform.position;
            fuckme.transform.position = new Vector3(posX, pos.y, pos.z);

            var lootView = fuckme.GetComponentInChildren<LootView>();

            lootView.SetModel(lootModel);
        }

        #endregion
    }
}

