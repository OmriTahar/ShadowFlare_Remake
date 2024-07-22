using ShadowFlareRemake.Enemies;
using UnityEngine;

namespace ShadowFlareRemake.GameManager.Units
{
    public class EnemyToSpawn : MonoBehaviour
    {
        [field: SerializeField] public EnemyController EnemyPrefab { get; private set; }
        [field: SerializeField] public EnemyUnitStats EnemyUnit { get; private set; }

        public void SetGameobjectNameAsEnemy()
        {
            if(EnemyUnit == null)
                return;

            name = EnemyUnit.Name;
        }

#if UNITY_EDITOR

        [UnityEditor.CustomEditor(typeof(EnemyToSpawn))]
        public class Drawer : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                GUILayout.Space(10);

                if(GUILayout.Button("Set Gameobject's Name As Enemy"))
                {
                    var enemyToSpawn = target as EnemyToSpawn;
                    enemyToSpawn.SetGameobjectNameAsEnemy();
                }
            }
        }

#endif

    }

}
