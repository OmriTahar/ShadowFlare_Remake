using UnityEngine;

namespace ShadowFlareRemake.Tests {
    public class TestController : MonoBehaviour {

        [Header("References")]
        [SerializeField] private Transform _spawnParent;
        [SerializeField] private GameObject _enemyPrefab;
        public void SpawnEnemy() {
            Instantiate(_enemyPrefab, _spawnParent);
        }

#if UNITY_EDITOR

        [UnityEditor.CustomEditor(typeof(TestController))]
        public class Drawer : UnityEditor.Editor {

            public override void OnInspectorGUI() {

                base.OnInspectorGUI();

                GUILayout.Space(10);

                if(GUILayout.Button("Spawn Enemy")) {
                    var tester = target as TestController;
                    tester.SpawnEnemy();
                }
            }
        }

#endif
    }
}

