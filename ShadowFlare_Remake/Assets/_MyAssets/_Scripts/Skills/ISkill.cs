using UnityEngine;

namespace ShadowFlareRemake.Skills
{
    public interface ISkill
    {
        public SkillType SkillType { get; }
        public Sprite Sprite { get; }
        public int MpCost { get; }
        public int Damage { get; }
    }
}
