using ShadowFlareRemake.Player;
using ShadowFlareRemake.Units;

namespace ShadowFlareRemake.UI.Stats
{
    public class StatsModel : Model
    {
        public IUnit Unit { get; private set; }
        public IPlayerUnitStats Stats { get; private set; }
        public IEquippedGearAddedStats EquippedGearAddedStats { get; private set; }

        public bool IsPanelActive { get; private set; }
        public bool IsFullStatsUpdate { get; private set; } = true;

        public StatsModel(bool isPanelOpen)
        {
            IsPanelActive = isPanelOpen;
        }

        public void InitPlayerFullStats(IUnit unit, IEquippedGearAddedStats addedStats)
        {
            Unit = unit;
            Stats = unit.Stats as IPlayerUnitStats;
            EquippedGearAddedStats = addedStats;
            Changed();
        }

        public void SetIsStatsPanelActive(bool isPanelOpen)
        {
            IsPanelActive = isPanelOpen;
            Changed();
        }

        public void SetFullPlayerStats()
        {
            SetIsFullStatsUpdate(true);
            Changed();
        }

        public void SetPlayerStats(bool isFullUpdate)
        {
            SetIsFullStatsUpdate(isFullUpdate);
            Changed();
        }

        private void SetIsFullStatsUpdate(bool isFullStatsUpdate)
        {
            IsFullStatsUpdate = isFullStatsUpdate;
        }
    }
}
