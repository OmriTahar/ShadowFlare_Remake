using UnityEngine;

namespace ShadowFlareRemake.Enemies {
    public class EnemyToSpawn : MonoBehaviour {
        [field: SerializeField] public EnemyController EnemyController { get; private set; }
        [field: SerializeField] public EnemyStats EnemyStats { get; private set; }
    }
}
