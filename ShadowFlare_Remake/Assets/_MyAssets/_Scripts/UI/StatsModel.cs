
using ShadowFlareRemake.Player;

namespace ShadowFlareRemake.UI {
    public class StatsModel : Model {

        public bool IsStatsOpen { get; private set; }

        public IUnit Unit { get; private set; }
        public IPlayerStats Stats { get; private set; }

        public StatsModel(bool isInventoryOpen) {

            IsStatsOpen = isInventoryOpen;
        }

        public void SetPlayerStats(IUnit unit) {

            Unit = unit;
            Stats = unit.Stats as IPlayerStats;
            Changed();
        }

        public void SetIsStatsOpen(bool isInventoryOpen) {

            IsStatsOpen = isInventoryOpen;
            Changed();
        }
    }
}
