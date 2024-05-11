using ShadowFlareRemake.Player;

namespace ShadowFlareRemake.UI {
    public class StatsModel : Model {

        public IUnit Unit { get; private set; }
        public IPlayerUnitStats Stats { get; private set; }

        public bool IsPanelOpen { get; private set; }

        public StatsModel(bool isPanelOpen) {

            IsPanelOpen = isPanelOpen;
        }

        public void SetPlayerStats(IUnit unit) {

            Unit = unit;
            Stats = unit.Stats as IPlayerUnitStats;
            Changed();
        }

        public void SetIsStatsOpen(bool isPanelOpen) {

            IsPanelOpen = isPanelOpen;
            Changed();
        }
    }
}
