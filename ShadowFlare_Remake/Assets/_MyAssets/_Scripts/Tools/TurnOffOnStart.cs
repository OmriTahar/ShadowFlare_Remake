using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Tools {
    public class TurnOffOnStart : MonoBehaviour {

        [SerializeField] private List<GameObject> gameObjectsToTurnOff;

        void Start() {
            foreach(GameObject go in gameObjectsToTurnOff) {
                go.SetActive(false);
            }
        }
    }
}

