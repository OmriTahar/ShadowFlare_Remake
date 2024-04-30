namespace ShadowFlareRemake.Combat {
    public static class CombatLogic {

        public static void HandleTakeDamage(Attack attack, IUnit receiverUnit, IUnitHandler receiverUnitHandler) {

            var damage = 0;

            if(attack.AttackType is Enums.AttackType.Physical) {

                damage = GetPhysicalDamage(attack.Unit, receiverUnit);

            } else if(attack.AttackType is Enums.AttackType.Magical) {

                damage = GetMagicalDamage(attack.Unit, receiverUnit);
            }
           
            receiverUnitHandler.TakeDamage(damage);
        }

        private static int GetPhysicalDamage(IUnit Attacker, IUnit receiverUnit) { // Todo: Expand this.

            var damage = Attacker.Attack - receiverUnit.Defense;
            return damage > 1 ? damage : 1;
        }

        private static int GetMagicalDamage(IUnit Attacker, IUnit receiverUnit) {  // Todo: Expand this.

            var damage = Attacker.MagicalAttack - receiverUnit.MagicalDefence;
            return damage > 1 ? damage : 1;
        }
    }
}
