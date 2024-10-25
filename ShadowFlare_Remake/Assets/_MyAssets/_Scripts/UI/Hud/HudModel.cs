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
        public SkillModel ActiveSkill { get; private set; }
        public SkillsBarPosition CurrentSkillsBarPosition { get; private set; }

        public bool IsCloseButtonActive { get; private set; }

        private const int _skillsAmount = 9;

        #region Initialization

        public HudModel()
        {
            InitSkillModels();
        }

        private void InitSkillModels()
        {
            SkillModels = new List<SkillModel>(_skillsAmount);

            for(int i = 0; i < _skillsAmount; i++)
            {
                SkillModels.Add(new SkillModel(null));
            }
        }

        #endregion

        #region HUD Panel

        public void SetHPAndMP(int currentHP, int maxHP, int currentMP, int maxMP)
        {
            CurrentHpEffectSlider = currentHP > CurrentHP ? SliderEffectType.Restore : SliderEffectType.Reduce;
            CurrentMpEffectSlider = currentMP > CurrentMP ? SliderEffectType.Restore : SliderEffectType.Reduce;

            if(!IsVitalsChanged(currentHP, maxHP, currentMP, maxMP))
                return;

            CurrentHP = currentHP;
            MaxHP = maxHP;
            CurrentMP = currentMP;
            MaxMP = maxMP;
            Changed();
        }

        public void SetExp(int currentExp, int expToNextLevel)
        {
            var isExpChanged = CurrentExp != currentExp || ExpToLevelUp != expToNextLevel;

            if(!isExpChanged)
                return;

            CurrentExp = currentExp;
            ExpToLevelUp = expToNextLevel;
            Changed();
        }

        public void SetLevel(int newLevel)
        {
            if(Level == newLevel)
                return;

            Level = newLevel;
            Changed();
        }

        public void SetIsCloseButtonActive(bool isCloseButtonActive, bool InvokeChanged = false)
        {
            if(IsCloseButtonActive == isCloseButtonActive)
                return;

            IsCloseButtonActive = isCloseButtonActive;

            if(InvokeChanged)
            {
                Changed();
            }
        }

        private bool IsVitalsChanged(int currentHP, int maxHP, int currentMP, int maxMP)
        {
            var isTheSameHP = CurrentHP == currentHP && MaxHP == maxHP;
            var isTheSameMP = CurrentMP == currentMP && MaxMP == maxMP;

            if(isTheSameHP && isTheSameMP)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Skills Bar

        public void SetSkillsBarPosition(SkillsBarPosition skillsBarPosition)
        {
            if(CurrentSkillsBarPosition == skillsBarPosition)
                return;

            CurrentSkillsBarPosition = skillsBarPosition;
            Changed();
        }

        public void SetPlayerSkills(List<ISkillData> playerSkills)
        {
            ISkillData meleeSkill = null;
            var barIndex = 0;

            foreach(var skill in playerSkills)
            {
                if(skill.SkillType == SkillType.MeleeAttack)
                {
                    meleeSkill = skill;
                    continue;
                }

                SkillModels[barIndex].SetSkillData(skill);
                barIndex++;
            }

            SkillModels[_skillsAmount - 1].SetSkillData(meleeSkill);
            Changed();
        }

        public void SetActiveSkill(SkillType skillType)
        {
            var skillModel = GetSkillModelFromSkillType(skillType);

            if(skillModel == null)
                return;

            if(ActiveSkill != null)
            {
                if(ActiveSkill.SkillData != null && ActiveSkill.SkillData.SkillType == skillType)
                    return;

                ActiveSkill.SetIsSelected(false);
            }

            ActiveSkill = skillModel;
            ActiveSkill.SetIsSelected(true);
        }

        private SkillModel GetSkillModelFromSkillType(SkillType skillType)
        {
            foreach(var skillModel in SkillModels)
            {
                if(skillModel.SkillData == null)
                {
                    continue;
                }

                if(skillModel.SkillData.SkillType == skillType)
                {
                    return skillModel;
                }
            }

            return null;
        }

        #endregion
    }
}


