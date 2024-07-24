using ShadowFlareRemake.Skills;
using System.Collections.Generic;

namespace ShadowFlareRemake.Units
{
    public interface IUnitSkills
    {
        public List<ISkillData> Skills { get; }
    }
}
