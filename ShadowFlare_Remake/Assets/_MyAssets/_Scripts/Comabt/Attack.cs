using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Combat
{
    public class Attack : MonoBehaviour
    {
        [field: SerializeField] public SkillDamageType DamageType { get; private set; }

        public IUnitStats Stats { get; private set; }

        public void SetUnitStats(IUnitStats stats)
        {
            Stats = stats;
        }
    }
}

