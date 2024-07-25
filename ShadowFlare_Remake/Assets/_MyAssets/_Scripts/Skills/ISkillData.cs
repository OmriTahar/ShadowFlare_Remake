using UnityEngine;

namespace ShadowFlareRemake.Skills
{
    public interface ISkillData
    {
        public SkillType SkillType { get; }
        public SkillDamageType DamageType { get; }
        public Sprite Sprite { get; }
        public int MpCost { get; }
        public int AddedDamage { get; }
    }
}
