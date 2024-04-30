using UnityEngine;
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Combat {
    public class Attack : MonoBehaviour {

        [field: SerializeField] public AttackType AttackType { get; private set; }

        public IUnit Unit { get; private set; } 

        public void SetUnitStats(IUnit unit) {

            Unit = unit;
        }
    }
}

