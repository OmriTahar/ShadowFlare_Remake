
namespace ShadowFlareRemake {

    public class Unit : IUnit {

        public UnitStats Stats { get; private set; }
        public int CurrentHP { get; private set; }
        public int CurrentMP { get; private set; }

        public Unit(IUnit iUnit) {
            Stats = iUnit.Stats;
        }

        public void Init() {
            FullHeal(false);
        }

        //protected virtual void Awake() {
        //    FullHeal(false);
        //}

        protected virtual void FullHeal(bool notifyUiController) {

            CurrentHP = Stats.MaxHP;
            CurrentMP = Stats.MaxMP;
        }

        public virtual void TakeDamage(int damage) {

            CurrentHP -= damage;
        }

        public virtual void HealHP(int hpAmount) {

            if(CurrentHP + hpAmount > Stats.MaxHP) {
                CurrentHP = Stats.MaxHP;
            } else {
                CurrentHP += hpAmount;
            }
        }

        public virtual void HealMP(int mpAmount) {

            if(CurrentMP + mpAmount > Stats.MaxMP) {
                CurrentMP = Stats.MaxMP;
            } else {
                CurrentMP += mpAmount;
            }
        }
    }
}

