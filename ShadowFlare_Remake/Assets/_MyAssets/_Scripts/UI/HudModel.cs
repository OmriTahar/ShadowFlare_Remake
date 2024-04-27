
namespace ShadowFlareRemake.UI {
    public class HudModel : Model {

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int CurrentMP { get; private set; }
        public int MaxMP { get; private set; }

        public int CurrentExp { get; private set; }
        public int ExpToLevelUp { get; private set; }
        public int Level { get; private set; }

        public HudModel() {
        }

        public void SetHPAndMP(int currentHP, int maxHP, int currentMP, int maxMP, bool invokeChanged = true) {

            CurrentHP = currentHP;
            MaxHP = maxHP;
            CurrentMP = currentMP;
            MaxMP = maxMP;

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


