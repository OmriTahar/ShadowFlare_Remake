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

            var inflictedDamage = GetInflictedDamage(skill, attack.AttackerStats, receiverStats);
            var isCriticalHit = GetIsCritialHit();

            return new ReceivedAttackData(inflictedDamage, isCriticalHit);
        }
        private int GetInflictedDamage(ISkillData skill, IUnitStats attackerStats, IUnitStats receiverStats)
        {
            var inflictedDamage = skill.AddedDamage;

            if(skill.DamageType is SkillDamageType.Physical)
            {
                inflictedDamage += GetPhysicalDamage(attackerStats, receiverStats);
            }
            else if(skill.DamageType is SkillDamageType.Magical)
            {
                inflictedDamage += GetMagicalDamage(attackerStats, receiverStats);
            }

            return inflictedDamage;
        }

        private int GetPhysicalDamage(IUnitStats AttackerStats, IUnitStats receiverStats) // Todo: Expand this.
        {
            return AttackerStats.Attack - receiverStats.Defense;
        }

        private int GetMagicalDamage(IUnitStats Attacker, IUnitStats receiverUnit) // Todo: Expand this.
        {
            return Attacker.MagicalAttack - receiverUnit.MagicalDefense;
        }

        private bool GetIsCritialHit()
        {
            return Random.value >= 0.75f; // Bruh
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
