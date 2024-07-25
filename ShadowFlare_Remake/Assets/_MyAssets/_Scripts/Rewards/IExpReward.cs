
namespace ShadowFlareRemake.Rewards
{
    public interface IExpReward
    {
        public int NewCurrentExp { get; }
        public int NewExpToLevelUp { get; }
        public int NewLevel { get; }
        public bool IsPendingLevelUp { get; }
    }
}
