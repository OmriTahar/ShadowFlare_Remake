using ShadowFlareRemake.Skills;
using System.Collections.Generic;

namespace ShadowFlareRemake.UI.Hud
{
    public class HudModel : Model
    {
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public SliderEffectType CurrentHpEffectSlider { get; private set; } = SliderEffectType.Reduce;

        public int CurrentMP { get; private set; }
        public int MaxMP { get; private set; }
        public SliderEffectType CurrentMpEffectSlider { get; private set; } = SliderEffectType.Reduce;

        public int CurrentExp { get; private set; }
        public int ExpToLevelUp { get; private set; }
        public int Level { get; private set; }

        public List<SkillModel> SkillModels { get; private set; }
        public SkillType ActiveSkill { get; private set; }

        public bool IsCloseButtonActive { get; private set; }

        #region Initialization

        public HudModel() { }

        #endregion

        #region Meat & Potatos

        public void SetHPAndMP(int currentHP, int maxHP, int currentMP, int maxMP)
        {
            CurrentHpEffectSlider = currentHP > CurrentHP ? SliderEffectType.Fill : SliderEffectType.Reduce;

            CurrentHP = currentHP;
            MaxHP = maxHP;
            CurrentMP = currentMP;
            MaxMP = maxMP;

            Changed();
        }

        public void SetExp(int currentExp, int expToNextLevel, bool invokeChanged = true)
        {
            CurrentExp = currentExp;
            ExpToLevelUp = expToNextLevel;
            Changed();
        }

        public void SetLevel(int newLevel, bool invokeChanged = true)
        {
            Level = newLevel;
            Changed();
        }

        public void SetIsCloseButtonActive(bool isCloseButtonActive)
        {
            IsCloseButtonActive = isCloseButtonActive;
            Changed();
        }

        public void SetSkillModels(List<SkillModel> skillModels)
        {
            SkillModels = skillModels;
            Changed();
        }

        public void SetActiveSkill(SkillType activeSkill)
        {
            ActiveSkill = activeSkill;
            Changed();
        }

        #endregion
    }
}


