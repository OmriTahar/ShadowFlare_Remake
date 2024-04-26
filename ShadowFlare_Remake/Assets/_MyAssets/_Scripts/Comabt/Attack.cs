using UnityEngine;
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Combat {
    public class Attack : MonoBehaviour {

        [field: SerializeField] public AttackType AttackType { get; private set; }

        public IUnitStats UnitStats { get; private set; } 

        public void SetUnitStats(IUnitStats unitStats) {
            
            UnitStats = unitStats;
        }
    }
}

