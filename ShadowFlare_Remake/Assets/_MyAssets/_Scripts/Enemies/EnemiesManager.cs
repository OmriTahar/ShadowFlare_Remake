using ShadowFlareRemake.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public class EnemiesManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private EnemyController[] _enemyControllers;

        private Dictionary<IUnit, Unit> _unitsDict = new();

        private void Awake() {

            foreach(var enemy in _enemyControllers) {

                enemy.Init();
                enemy.InitEnemy(_playerTransform);

                enemy.OnIGotHit += HandleEnemyGotHit;

                _unitsDict.Add(enemy.Unit, new Unit(enemy.Unit));
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
