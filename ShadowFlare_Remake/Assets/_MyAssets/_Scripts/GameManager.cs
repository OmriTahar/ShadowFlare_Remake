using UnityEngine;

namespace ShadowFlareRemake {
    public class GameManager : MonoBehaviour {

        public static GameManager Instance { get; private set; }
        [field: SerializeField] public Transform PlayerTransform { get; private set; }

        private void Awake() {

            InitSingelton();
        }

        private void InitSingelton() {

            if(Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            } else if(this != Instance) {
                Destroy(this);
            }
        }
    }
}

