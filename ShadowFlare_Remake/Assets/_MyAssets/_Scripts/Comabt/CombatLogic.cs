namespace ShadowFlareRemake.Combat {
    public static class CombatLogic {

        public static void HandleTakeDamage(Attack attack, IUnit receiverUnit) {

            var damage = 0;

            if(attack.AttackType is Enums.AttackType.Physical) {

                damage = GetPhysicalDamage(attack.Stats, receiverUnit.Stats);

            } else if(attack.AttackType is Enums.AttackType.Magical) {

                damage = GetMagicalDamage(attack.Stats, receiverUnit.Stats);
            }
           
            receiverUnit.TakeDamage(damage);
        }

        private static int GetPhysicalDamage(IUnitStats AttackerStats, IUnitStats receiverStats) { // Todo: Expand this.

            var damage = AttackerStats.Attack - receiverStats.Defense;
            return damage > 1 ? damage : 1;
        }

        private static int GetMagicalDamage(IUnitStats Attacker, IUnitStats receiverUnit) {  // Todo: Expand this.

            var damage = Attacker.MagicalAttack - receiverUnit.MagicalDefence;
            return damage > 1 ? damage : 1;
        }
    }
}
