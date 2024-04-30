using UnityEngine;
using ShadowFlareRemake.Enemies;

namespace ShadowFlareRemake.GameManager {
    public class EnemyToSpawn : MonoBehaviour {
        [field: SerializeField] public EnemyController EnemyPrefab { get; private set; }
        [field: SerializeField] public EnemyUnit EnemyUnit { get; private set; }
    }
}
