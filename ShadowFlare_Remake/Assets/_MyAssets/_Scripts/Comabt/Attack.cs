using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Combat
{
    public class Attack : MonoBehaviour
    {
        [field: SerializeField] public SkillType SkillType { get; private set; }

        public IUnitStats AttackerStats { get; private set; }

        public void SetUnitStats(IUnitStats stats)
        {
            AttackerStats = stats;
        }
    }
}

