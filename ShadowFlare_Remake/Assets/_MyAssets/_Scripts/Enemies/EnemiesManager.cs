using ShadowFlareRemake.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public class EnemiesManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _enemiesParent;
        [Space(10)]
        [SerializeField] private EnemyToSpawn[] _enemiesToSpawn;

        private Dictionary<IUnit, Unit> _unitsDict = new();
        private List<EnemyController> _enemyControllers = new();

        private void Awake() {

            foreach(var enemyToSpawn in _enemiesToSpawn) {

                if(enemyToSpawn == null) {
                    Debug.LogError("Enemies Manager - EnemyToSpawn Null Reference!");
                }

                var spawnPoint = enemyToSpawn.transform;
                var enemy = Instantiate(enemyToSpawn.EnemyController, spawnPoint.position, spawnPoint.rotation,_enemiesParent);

                enemy.OnIGotHit += HandleEnemyGotHit;
                _enemyControllers.Add(enemy);

                var newUnit = new Unit(enemyToSpawn.EnemyStats);

                enemy.Init();
                enemy.InitEnemy(newUnit, _playerTransform);

                _unitsDict.Add(enemy.Unit, newUnit);

                Destroy(enemyToSpawn.gameObject);
            }
        }

        private void OnDestroy() {

            foreach(var enemy in _enemyControllers) {

                enemy.OnIGotHit -= HandleEnemyGotHit;
            }
        }

        private void HandleEnemyGotHit(Attack attack, EnemyController enemyController) {

            var unit = _unitsDict[enemyController.Unit];

            CombatUtils.HandleTakeDamage(attack, _unitsDict[enemyController.Unit]);

            enemyController.SetUnit(unit);
        }
    }
}
