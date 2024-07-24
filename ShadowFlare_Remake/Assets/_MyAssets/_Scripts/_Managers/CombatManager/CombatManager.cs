using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Units;
using UnityEngine;

namespace ShadowFlareRemake.Managers.Combat
{
    public class CombatManager
    {
        public bool HandleTakeDamageAndReturnIsCritialHit(Attack attack, IUnit receiverUnit)
        {
            var damage = 0;

            if(attack.AttackType is AttackDamageType.Physical)
            {
                damage = GetPhysicalDamage(attack.Stats, receiverUnit.Stats);
            }
            else if(attack.AttackType is AttackDamageType.Magical)
            {
                damage = GetMagicalDamage(attack.Stats, receiverUnit.Stats);
            }

            receiverUnit.TakeDamage(damage);

            return Random.value >= 0.75f; // Improve this
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
