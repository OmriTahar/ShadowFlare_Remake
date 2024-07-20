using ShadowFlareRemake.Combat;
using UnityEngine;

namespace ShadowFlareRemake.GameManager
{
    public static class CombatManager
    {
        public static bool HandleTakeDamageAndReturnIsCritialHit(Attack attack, IUnit receiverUnit)
        {
            var damage = 0;

            if(attack.AttackType is Enums.AttackType.Physical)
            {

                damage = GetPhysicalDamage(attack.Stats, receiverUnit.Stats);

            }
            else if(attack.AttackType is Enums.AttackType.Magical)
            {

                damage = GetMagicalDamage(attack.Stats, receiverUnit.Stats);
            }

            receiverUnit.TakeDamage(damage);

            return Random.value >= 0.75f; // Improve this
        }

        private static int GetPhysicalDamage(IUnitStats AttackerStats, IUnitStats receiverStats) // Todo: Expand this.
        { 

            var damage = AttackerStats.Attack - receiverStats.Defense;
            return damage > 1 ? damage : 1;
        }

        private static int GetMagicalDamage(IUnitStats Attacker, IUnitStats receiverUnit) // Todo: Expand this.
        {  

            var damage = Attacker.MagicalAttack - receiverUnit.MagicalDefense;
            return damage > 1 ? damage : 1;
        }
    }
}
