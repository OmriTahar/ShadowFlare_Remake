using System.Collections.Generic;
using UnityEngine;
using ShadowFlareRemake.Combat;
using System;

namespace ShadowFlareRemake.Enemies {
    public class EnemiesManager : MonoBehaviour {

        public event Action<IUnitStats> OnEnemyDied;

        [Header("References")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _enemiesParent;

        private Dictionary<IUnit, Unit> _unitsDict = new();
        private List<EnemyController> _enemyControllers = new();

        private void Awake() {

            InitEnemies();
        }

        private void OnDestroy() {

            DeregisterEvents();
        }

        private void InitEnemies() {

            var enemiesToSpawn = _enemiesParent.GetComponentsInChildren<EnemyToSpawn>();
            
            foreach(var enemyToSpawn in enemiesToSpawn) {

                if(enemyToSpawn == null) {
                    Debug.LogError("Enemies Manager - EnemyToSpawn Null Reference!");
                    continue;
                }

                var spawnPoint = enemyToSpawn.transform;
                var enemy = Instantiate(enemyToSpawn.EnemyPrefab, spawnPoint.position, spawnPoint.rotation, _enemiesParent);

                enemy.OnIGotHit += HandleEnemyGotHit;
                enemy.OnIGotKilled += HandleEnemyDied;

                var newUnit = new Unit(enemyToSpawn.EnemyStats);
                enemy.InitEnemy(newUnit, _playerTransform);

                _enemyControllers.Add(enemy);
                _unitsDict.Add(enemy.Unit, newUnit);

                Destroy(enemyToSpawn.gameObject);
            }
        }

        private void DeregisterEvents() {

            foreach(var enemy in _enemyControllers) {

                enemy.OnIGotHit -= HandleEnemyGotHit;
                enemy.OnIGotKilled -= HandleEnemyDied;
            }
        }

        private void HandleEnemyGotHit(Attack attack, EnemyController enemyController) {

            var unit = _unitsDict[enemyController.Unit];

            CombatUtils.HandleTakeDamage(attack, _unitsDict[enemyController.Unit]);

            enemyController.SetUnit(unit);
        }

        private void HandleEnemyDied(IUnitStats unitStats) {

            OnEnemyDied.Invoke(unitStats);
        }
    }
}
