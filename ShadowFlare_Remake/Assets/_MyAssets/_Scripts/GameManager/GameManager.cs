using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Player;
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

        private Dictionary<EnemyController, Unit> _enemyUnitsDict = new();
        private Unit _playerUnit;

        #region Unity Callbacks

        private  void Awake() {

            DontDestroyOnLoad(gameObject);
        }

        private async void Start() {

            await InitPlayer();
            InitEnemies();
            RegisterEvents();

            _uiController.UpdatePlayerStats(_playerUnit);
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

                if(enemyToSpawn == null) {
                    Debug.LogError("Enemies Manager - EnemyToSpawn Null Reference!");
                    continue;
                }

                var spawnPoint = enemyToSpawn.transform;
                var enemyController = Instantiate(enemyToSpawn.EnemyPrefab, spawnPoint.position, spawnPoint.rotation, _enemiesParent);

                var enemyUnitStats = enemyToSpawn.EnemyUnit;
                var enemyUnit = new Unit(enemyUnitStats);

                enemyController.InitEnemy(enemyUnit, _playerController.transform);
                _enemyUnitsDict.Add(enemyController, enemyUnit);

                RegisterEnemyEvents(enemyController);
                Destroy(enemyToSpawn.gameObject);
            }
        }

        private void RegisterEnemyEvents(EnemyController enemyController) {

            enemyController.OnIGotHit += HandleEnemyGotHit;
            enemyController.OnIGotKilled += HandleEnemyDied;
        }

        private void HandleEnemyGotHit(Attack attack, EnemyController enemyController) {

            var unit = _enemyUnitsDict[enemyController];

            CombatLogic.HandleTakeDamage(attack, unit);

            enemyController.SetEnemyUnitAndUnitHandler(unit);
        }

        private void HandleEnemyDied(IEnemyUnitStats enemyStats) {

            var playerStats = _playerUnit.Stats as PlayerUnitStats;
            var expReward = _rewardsManager.GetExpReward(playerStats, enemyStats);

            playerStats.GiveExpReward(expReward);
            _uiController.UpdatePlayerStats(_playerUnit);
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

