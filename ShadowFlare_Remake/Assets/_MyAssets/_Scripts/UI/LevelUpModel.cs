using ShadowFlareRemake.Rewards;

namespace ShadowFlareRemake.UI {
    public class LevelUpModel : Model {

        public bool IsPanelOpen { get; private set; }

        public int Level { get; private set; } 
        public int HP { get; private set; }
        public int MP { get; private set; }
        public int Strength { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int MagicalAttack { get; private set; }
        public int MagicalDefence { get; private set; }

        public LevelUpModel() { }

        public void SetReward(int newLevel ,ILevelUpReward reward, bool isPanelOpen) {

            IsPanelOpen = isPanelOpen;

            Level = newLevel;
            HP = reward.HP;
            MP = reward.MP;
            Strength = reward.Strength;
            Attack = reward.Attack;
            Defense = reward.Defense;
            MagicalAttack = reward.MagicalAttack;
            MagicalDefence = reward.MagicalDefence;
            Changed();
        }

        public void SetIsPanelOpen(bool isPanelOpen) {

            IsPanelOpen = isPanelOpen;
            Changed();
        }
    }
}
