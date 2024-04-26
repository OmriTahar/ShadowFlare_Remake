namespace ShadowFlareRemake.Combat {
    public static class CombatUtils {

        public static void HandleTakeDamage(Attack attack, Unit Reciever) {

            var damage = 0;

            if(attack.AttackType == Enums.AttackType.Physical) {

                damage = GetPhysicalDamage(attack.UnitStats, Reciever.Stats);

            } else {
                damage = GetMagicalDamage(attack.UnitStats, Reciever.Stats);
            }

            Reciever.TakeDamage(damage);
        }

        private static int GetPhysicalDamage(UnitStats Attacker, UnitStats Reciever) { // Todo: Expand this.

            var damage = Attacker.Attack - Reciever.Defence;
            return damage > 1 ? damage : 1;
        }

        private static int GetMagicalDamage(UnitStats Attacker, UnitStats Reciever) { // Todo: Expand this.

            var damage = Attacker.MagicalAttack - Reciever.MagicalDefence;
            return damage > 1 ? damage : 1;
        }
    }
}
