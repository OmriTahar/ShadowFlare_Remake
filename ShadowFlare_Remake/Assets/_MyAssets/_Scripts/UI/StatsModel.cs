using ShadowFlareRemake.Player;

namespace ShadowFlareRemake.UI {
    public class StatsModel : Model {

        public IUnit Unit { get; private set; }
        public IPlayerUnitStats Stats { get; private set; }

        public bool IsStatsOpen { get; private set; }

        public StatsModel(bool isInventoryOpen) {

            IsStatsOpen = isInventoryOpen;
        }

        public void SetPlayerStats(IUnit unit) {

            Unit = unit;
            Stats = unit.Stats as IPlayerUnitStats;
            Changed();
        }

        public void SetIsStatsOpen(bool isInventoryOpen) {

            IsStatsOpen = isInventoryOpen;
            Changed();
        }
    }
}
