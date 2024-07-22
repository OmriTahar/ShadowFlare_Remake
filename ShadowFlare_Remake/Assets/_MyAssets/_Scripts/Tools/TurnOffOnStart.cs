using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Tools
{
    public class TurnOffOnStart : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _gameObjectsToTurnOff;

        void Start()
        {
            foreach(GameObject gameObject in _gameObjectsToTurnOff)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

