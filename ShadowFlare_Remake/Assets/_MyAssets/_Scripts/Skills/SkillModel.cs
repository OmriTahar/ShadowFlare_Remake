using UnityEngine;

namespace ShadowFlareRemake.Skills
{
    public class SkillModel : Model
    {
        public SkillData_ScriptableObject SkillData { get; private set; }

        public SkillModel(SkillData_ScriptableObject skillData)
        {
            SkillData = skillData;
        }
    }
}
