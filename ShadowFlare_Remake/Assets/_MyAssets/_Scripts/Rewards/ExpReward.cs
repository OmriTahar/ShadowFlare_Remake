
namespace ShadowFlareRemake.Rewards
{
    public struct ExpReward
    {
        public int NewCurrentExp {  get; private set; }
        public int NewExpToLevelUp { get; private set; }
        public int NewLevel { get; private set; }
        public bool IsPendingLevelUp { get; private set; }

        public ExpReward(int newCurrentExp, int newExpToLevelUp, int newLevel, bool isPendingLevelUp)
        {
            NewCurrentExp = newCurrentExp;
            NewExpToLevelUp = newExpToLevelUp;
            NewLevel = newLevel;
            IsPendingLevelUp = isPendingLevelUp;
        }
    }
}
