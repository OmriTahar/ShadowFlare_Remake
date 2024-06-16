using ShadowFlareRemake.Enemies;
using UnityEngine;

namespace ShadowFlareRemake.GameManager.Units
{
    public class EnemyToSpawn : MonoBehaviour
    {
        [field: SerializeField] public EnemyController EnemyPrefab { get; private set; }
        [field: SerializeField] public EnemyUnitStats EnemyUnit { get; private set; }
    }
}
