using UnityEngine;
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Combat {
    public class Attack : MonoBehaviour {
        [field: SerializeField] public AttackType AttackType { get; private set; }
        [field: SerializeField] public IUnitStats UnitStats { get; private set; } 
    }
}

