using ShadowFlareRemake.EnemiesRestrictedData;
using ShadowFlareRemake.UnitsRestrictedData;

namespace ShadowFlareRemake.GameManagerRestrictedData
{
    public class EnemyConcreteData
    {
        public Unit EnemyUnit {  get; private set; }
        public EnemyModel EnemyModel { get; private set; }

        public EnemyConcreteData(Unit enemyUnit, EnemyModel enemyModel)
        {
            EnemyUnit = enemyUnit;
            EnemyModel = enemyModel;
        }
    }
}
