using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Managers.Combat
{
    public class CombatManager
    {
        public ReceivedAttackData GetReceivedAttackData(Attack attack, IUnitStats receiverStats)
        {
            var inflictedDamage = 0;
            var isCriticalHit = Random.value >= 0.75f; // Improve this

            if(attack.DamageType is SkillDamageType.Physical)
            {
                inflictedDamage = GetPhysicalDamage(attack.Stats, receiverStats);
            }
            else if(attack.DamageType is SkillDamageType.Magical)
            {
                inflictedDamage = GetMagicalDamage(attack.Stats, receiverStats);
            }

            return new ReceivedAttackData(inflictedDamage, isCriticalHit);
        }

        private int GetPhysicalDamage(IUnitStats AttackerStats, IUnitStats receiverStats) // Todo: Expand this.
        { 
            var damage = AttackerStats.Attack - receiverStats.Defense;
            return damage > 1 ? damage : 1;
        }

        private int GetMagicalDamage(IUnitStats Attacker, IUnitStats receiverUnit) // Todo: Expand this.
        {  
            var damage = Attacker.MagicalAttack - receiverUnit.MagicalDefense;
            return damage > 1 ? damage : 1;
        }
    }
}
