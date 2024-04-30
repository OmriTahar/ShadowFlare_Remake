namespace ShadowFlareRemake.GameManager {

    public class UnitHandler : IUnitHandler {

        public int CurrentHP { get; private set; }

        public int CurrentMP { get; private set; }

        public UnitHandler(int maxHP, int maxMP) {

            CurrentHP = maxHP;
            CurrentMP = maxMP;
        }

        public void TakeDamage(int damage) {

            CurrentHP -= damage;
        }
    }
}
