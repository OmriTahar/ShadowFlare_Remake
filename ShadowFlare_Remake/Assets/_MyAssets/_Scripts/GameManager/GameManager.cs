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
        [SerializeField] private PlayerUnit _playerUnit;

        private Dictionary<EnemyController, EnemyUnit> _enemyUnitsDict = new();
        private Dictionary<EnemyController, UnitHandler> _enemyUnitHandlersDict = new();

        #region Unity Callbacks

        private async void Awake() {

            DontDestroyOnLoad(gameObject);
            await InitPlayer();
            InitEnemies();
        }

        private void Start() {

            RegisterEvents();
            _uiController.UpdatePlayerStats(_playerUnit, _playerUnit);
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

                var enemyUnit = enemyToSpawn.EnemyUnit;
                var enemyUnitHandler = new UnitHandler(enemyUnit.MaxHP, enemyUnit.MaxMP);

                enemyController.InitEnemy(enemyUnit, enemyUnitHandler, _playerController.transform);

                _enemyUnitsDict.Add(enemyController, enemyUnit);
                _enemyUnitHandlersDict.Add(enemyController, enemyUnitHandler);

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
            var unitHandler = _enemyUnitHandlersDict[enemyController];

            CombatLogic.HandleTakeDamage(attack, unit, unitHandler);

            enemyController.SetEnemyUnitAndUnitHandler(unit, unitHandler);
        }

        private void HandleEnemyDied(IEnemyUnit enemyUnit) {

            _rewardsManager.GiveRewardsToPlayer(_playerUnit, enemyUnit as EnemyUnit);
            _uiController.UpdatePlayerStats(_playerUnit, _playerUnit);
        }

        #endregion

        #region Player

        private async Task InitPlayer() {

            await _playerController.InitPlayer(_playerUnit, _playerUnit);
        }

        private void HandlePlayerGotHit(Attack attack, IUnit unit) {

        }

        #endregion
    }
}

