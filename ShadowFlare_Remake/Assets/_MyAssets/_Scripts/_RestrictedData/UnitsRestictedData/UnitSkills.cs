using ShadowFlareRemake.Skills;
using ShadowFlareRemake.SkillsRestrictedData;
using ShadowFlareRemake.Units;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UnitsRestrictedData
{
    public class UnitSkills : MonoBehaviour, IUnitSkills
    {
        public List<ISkillData> Skills {  get; private set; }   

        [Space(15)]
        [SerializeField] private string ______SKILLS_____ = _spaceLine;
        [SerializeField] private List<SkillData_ScriptableObject> _skills;

        private const string _spaceLine = "------------------------------------";

        private void Awake()
        {
            InitSkills();
        }

        private void InitSkills()
        {
            foreach(var skillData in _skills)
            {
                Skills.Add(skillData);
            }
        }
    }
}
