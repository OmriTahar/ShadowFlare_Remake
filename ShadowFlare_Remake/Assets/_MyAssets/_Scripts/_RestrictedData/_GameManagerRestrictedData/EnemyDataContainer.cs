using ShadowFlareRemake.EnemiesRestrictedData;
using ShadowFlareRemake.UnitsRestrictedData;

namespace ShadowFlareRemake.GameManagerRestrictedData
{
    public class EnemyDataContainer
    {
        public Unit EnemyUnit {  get; private set; }
        public EnemyModel EnemyModel { get; private set; }

        public EnemyDataContainer(Unit enemyUnit, EnemyModel enemyModel)
        {
            EnemyUnit = enemyUnit;
            EnemyModel = enemyModel;
        }
    }
}
