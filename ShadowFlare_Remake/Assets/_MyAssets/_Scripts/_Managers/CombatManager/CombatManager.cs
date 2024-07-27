using ShadowFlareRemake.Combat;
using ShadowFlareRemake.CombatRestrictedData;
using ShadowFlareRemake.Skills;
using ShadowFlareRemake.SkillsRestrictedData;
using ShadowFlareRemake.Units;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.CombatManagement
{
    public class CombatManager : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] private List<SkillData_ScriptableObject> _skills; // Todo: Get these automatically from folder

        private Dictionary<SkillType, SkillData_ScriptableObject> _skillsDict = new();

        private void Awake()
        {
            InitSkillsDict();
        }

        public ReceivedAttackData GetReceivedAttackData(Attack attack, IUnitStats receiverStats)
        {
            var skill = _skillsDict[attack.SkillType];

            if(skill == null || skill.DamageType == SkillDamageType.None)
            {
                return new ReceivedAttackData(0, false);
            }

            var inflictedDamage = skill.AddedDamage;
            var isCriticalHit = Random.value >= 0.75f; // Bruh

            if(skill.DamageType is SkillDamageType.Physical)
            {
                inflictedDamage += GetPhysicalDamage(attack.AttackerStats, receiverStats);
            }
            else if(skill.DamageType is SkillDamageType.Magical)
            {
                inflictedDamage += GetMagicalDamage(attack.AttackerStats, receiverStats);
            }

            return new ReceivedAttackData(inflictedDamage, isCriticalHit);
        }

        private int GetPhysicalDamage(IUnitStats AttackerStats, IUnitStats receiverStats) // Todo: Expand this.
        {
            return AttackerStats.Attack - receiverStats.Defense;
        }

        private int GetMagicalDamage(IUnitStats Attacker, IUnitStats receiverUnit) // Todo: Expand this.
        {
            return Attacker.MagicalAttack - receiverUnit.MagicalDefense;
        }

        private void InitSkillsDict()
        {
            foreach(var skill in _skills)
            {
                _skillsDict.Add(skill.SkillType, skill);
            }
        }

    }
}
