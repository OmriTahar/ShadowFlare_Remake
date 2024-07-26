using ShadowFlareRemake.EnemiesRestrictedData;
using ShadowFlareRemake.UnitsRestrictedData;
using UnityEngine;

namespace ShadowFlareRemake.GameManagerRestrictedData
{
    public class EnemyDataContainer
    {
        public Unit EnemyUnit {  get; private set; }
        public EnemyModel EnemyModel { get; private set; }
        public Collider EnemyCollider { get; private set; }

        public EnemyDataContainer(Unit enemyUnit, EnemyModel enemyModel, Collider enemyCollider)
        {
            EnemyUnit = enemyUnit;
            EnemyModel = enemyModel;
            EnemyCollider = enemyCollider;
        }
    }
}
