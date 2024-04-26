using UnityEngine;
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Combat {
    public class Attack : MonoBehaviour {
        [field: SerializeField] public AttackType AttackType { get; private set; }
        [field: SerializeField] public UnitStats UnitStats { get; private set; } 
    }
}

