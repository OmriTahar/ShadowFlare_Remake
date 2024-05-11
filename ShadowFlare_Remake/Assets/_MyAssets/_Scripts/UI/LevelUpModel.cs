using ShadowFlareRemake.Rewards;

namespace ShadowFlareRemake.UI {
    public class LevelUpModel : Model {

        public enum LevelUpPanelState { Hidden, Idle, Corner }
        public LevelUpPanelState State { get; private set; } = LevelUpPanelState.Hidden;

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

        public void SetReward(int newLevel, ILevelUpReward reward) {

            State = LevelUpPanelState.Idle;

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

        public void SetPanelState(LevelUpPanelState state) {

            State = state;
            Changed();
        }
    }
}
