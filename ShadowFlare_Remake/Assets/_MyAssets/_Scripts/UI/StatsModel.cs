using ShadowFlareRemake.Player;

namespace ShadowFlareRemake.UI {
    public class StatsModel : Model {

        public IPlayerUnit Unit { get; private set; }
        public IUnitHandler UnitHandler { get; private set; }

        public bool IsStatsOpen { get; private set; }

        public StatsModel(bool isInventoryOpen) {

            IsStatsOpen = isInventoryOpen;
        }

        public void SetPlayerStats(IPlayerUnit unit, IUnitHandler unitHandler) {

            Unit = unit;
            UnitHandler = unitHandler;
            Changed();
        }

        public void SetIsStatsOpen(bool isInventoryOpen) {

            IsStatsOpen = isInventoryOpen;
            Changed();
        }
    }
}
