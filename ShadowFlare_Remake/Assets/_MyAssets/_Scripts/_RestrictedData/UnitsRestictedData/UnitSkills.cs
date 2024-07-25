using ShadowFlareRemake.Skills;
using ShadowFlareRemake.SkillsRestrictedData;
using ShadowFlareRemake.Units;
using System.Collections.Generic;

namespace ShadowFlareRemake.UnitsRestrictedData
{
    public class UnitSkills : IUnitSkills
    {
        public List<ISkillData> Skills { get; private set; }

        public void SetSkills(List<SkillData_ScriptableObject> SkillsData)
        {
            Skills = new List<ISkillData>();

            foreach(var data in SkillsData)
            {
                Skills.Add(data);
            }
        }
    }
}
