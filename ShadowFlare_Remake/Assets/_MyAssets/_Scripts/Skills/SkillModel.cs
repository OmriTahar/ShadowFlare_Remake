
namespace ShadowFlareRemake.Skills
{
    public class SkillModel : Model
    {
        public ISkillData SkillData { get; private set; }

        public SkillModel(ISkillData skillData)
        {
            SkillData = skillData;
        }
    }
}
