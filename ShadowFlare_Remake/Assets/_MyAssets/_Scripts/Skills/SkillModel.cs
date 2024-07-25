
namespace ShadowFlareRemake.Skills
{
    public class SkillModel : Model
    {
        public ISkillData SkillData { get; private set; }
        public bool IsSelected { get; private set; }

        public SkillModel(ISkillData skillData)
        {
            SkillData = skillData;
        }

        public void SetIsSelected(bool isSelected)
        {
            IsSelected = isSelected;
            Changed();
        }
    }
}
