
namespace ShadowFlareRemake.UI {
    public class HudModel : Model {

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int CurrentMP { get; private set; }
        public int MaxMP { get; private set; }

        public int CurrentExp { get; private set; }
        public int ExpToLevelUp { get; private set; }
        public int Level { get; private set; }

        public HudModel(IUnitStats unitStats) {

            MaxHP = unitStats.MaxHP;
            MaxMP = unitStats.MaxMP;

            SetHPAndMP(MaxHP, MaxMP, false);
            //SetExp(unitStats.CurrentExp, unitStats.ExpToLevelUp, false);
            SetLevel(unitStats.Level, false);
        }

        public void SetHPAndMP(int currentHP, int currentMP, bool invokeChanged = true) {

            CurrentHP = currentHP;
            CurrentMP = currentMP;
            Changed();
        }

        public void SetExp(int currentExp, int expToNextLevel, bool invokeChanged = true) {

            CurrentExp = currentExp;
            ExpToLevelUp = expToNextLevel;
            Changed();
        }

        public void SetLevel(int newLevel, bool invokeChanged = true) {

            Level = newLevel;
            Changed();
        }
    }
}


