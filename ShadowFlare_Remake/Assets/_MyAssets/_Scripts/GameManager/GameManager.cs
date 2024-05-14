using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Player;
using ShadowFlareRemake.PlayerInput;
using ShadowFlareRemake.Rewards;
using ShadowFlareRemake.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowFlareRemake.GameManager {
    public class GameManager : MonoBehaviour {

        [Header("General")]
        [SerializeField] private RewardsManager _rewardsManager;
        [SerializeField] private UIController _uiController;

        [Header("Enemies")]
        [SerializeField] private Transform _enemiesParent;

        [Header("Player")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerUnitStats _playerUnitStats;

        private Unit _playerUnit;
        private Dictionary<EnemyController, Unit> _enemyUnitsDict = new();
        private Dictionary<Collider, EnemyModel> _enemiesCollidersDict = new();
        private EnemyModel _lastHighlightedEnemy;

        #region Unity Callbacks

        private void Awake() {

            DontDestroyOnLoad(gameObject);
        }

        private async void Start() {

            await InitPlayer();
            InitEnemies();
            RegisterEvents();

            _uiController.UpdatePlayerUI(_playerUnit);
        }

        private void Update() {

            HandleHighlightEnemies();
        }

        private void OnDestroy() {

            DergisterEvents();
        }

        #endregion

        #region Events

        private void RegisterEvents() {

            _playerController.OnIGotHit += HandlePlayerGotHit;
        }

        private void DergisterEvents() {

            _playerController.OnIGotHit -= HandlePlayerGotHit;

            foreach(var enemy in _enemyUnitsDict.Keys) {

                enemy.OnIGotHit -= HandleEnemyGotHit;
                enemy.OnIGotKilled -= HandleEnemyDied;
            }
        }

        #endregion

        #region Enemies

        private void InitEnemies() {

            var enemiesToSpawn = _enemiesParent.GetComponentsInChildren<EnemyToSpawn>();

            foreach(var enemyToSpawn in enemiesToSpawn) {

                InitEnemyLogic(enemyToSpawn);
            }
        }

        private void InitEnemyLogic(EnemyToSpawn enemyToSpawn) {

            if(enemyToSpawn == null) {
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

        private void RegisterEnemyEvents(EnemyController enemyController) {

            enemyController.OnIGotHit += HandleEnemyGotHit;
            enemyController.OnIGotKilled += HandleEnemyDied;
        }

        private void HandleHighlightEnemies() {

            if(InputManager.Instance.IsCursorOnEnemy) {

                var raycastHit = InputManager.Instance.CurrentRaycastHit;
                _enemiesCollidersDict.TryGetValue(raycastHit.collider, out var enemyModel);

                if(_lastHighlightedEnemy != null && enemyModel != _lastHighlightedEnemy) {
                    _lastHighlightedEnemy.SetIsEnemyHighlighted(false);
                }

                enemyModel.SetIsEnemyHighlighted(true);
                _lastHighlightedEnemy = enemyModel;
                return;

            }

            if(_lastHighlightedEnemy != null) {
                _lastHighlightedEnemy.SetIsEnemyHighlighted(false);
            }
        }

        private void HandleEnemyGotHit(Attack attack, EnemyController enemyController) {

            var unit = _enemyUnitsDict[enemyController];

            CombatLogic.HandleTakeDamage(attack, unit);

            enemyController.SetEnemyUnitAndUnitHandler(unit);
        }

        private void HandleEnemyDied(IEnemyUnitStats enemyStats) {

            var expReward = _rewardsManager.GetExpReward(_playerUnitStats, enemyStats);
            _playerUnitStats.GiveExpReward(expReward);

            if(expReward.IsPendingLevelUp) {

                var levelUpReward = _rewardsManager.GetLevelUpReward(_playerUnitStats);
                _playerUnitStats.GiveLevelUpReward(levelUpReward);
                _playerUnit.FullHeal();
                _uiController.ShowLevelUpPopup(_playerUnitStats.Level, levelUpReward);
            }

            _uiController.UpdatePlayerUI(_playerUnit);
        }

        #endregion

        #region Player

        private async Task InitPlayer() {

            _playerUnit = new Unit(_playerUnitStats);
            await _playerController.InitPlayer(_playerUnit);
        }

        private void HandlePlayerGotHit(Attack attack, IUnitStats unit) {

        }

        #endregion
    }
}

