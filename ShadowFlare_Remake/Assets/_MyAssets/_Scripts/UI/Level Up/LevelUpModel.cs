using ShadowFlareRemake.Rewards;

namespace ShadowFlareRemake.UI.LevelUp
{
    public class LevelUpModel : Model
    {
        public enum LevelUpPanelState { Hidden, Shown, MovingToCorner, FadingOut }
        public LevelUpPanelState State { get; private set; } = LevelUpPanelState.Hidden;

        public int Level { get; private set; }
        public int Strength { get; private set; }

        public int MaxHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int HitRate { get; private set; }
        public int EvasionRate { get; private set; }
        public int MovementSpeed { get; private set; }
        public int AttackSpeed { get; private set; }

        public int MaxMP { get; private set; }
        public int MagicalAttack { get; private set; }
        public int MagicalDefense { get; private set; }
        public int MagicalHitRate { get; private set; }
        public int MagicalEvasionRate { get; private set; }
        public int MagicalAttackSpeed { get; private set; }

        public LevelUpModel() { }

        public void SetPanelState(LevelUpPanelState state)
        {
            State = state;
            Changed();
        }

        public void SetReward(int newLevel, ILevelUpReward reward)
        {
            State = LevelUpPanelState.Shown;
            Level = newLevel;
            Strength += reward.Strength;

            MaxHP += reward.MaxHP;
            Attack += reward.Attack;
            Defense += reward.Defense;
            HitRate += reward.HitRate;
            EvasionRate += reward.EvasionRate;
            MovementSpeed += reward.MovementSpeed;
            AttackSpeed += reward.AttackSpeed;

            MaxMP += reward.MaxMP;
            MagicalAttack += reward.MagicalAttack;
            MagicalDefense += reward.MagicalDefense;
            MagicalHitRate += reward.MagicalHitRate;
            MagicalEvasionRate += reward.MagicalEvasionRate;
            MagicalAttackSpeed += reward.MagicalAttackSpeed;

            Changed();
        }
    }
}
