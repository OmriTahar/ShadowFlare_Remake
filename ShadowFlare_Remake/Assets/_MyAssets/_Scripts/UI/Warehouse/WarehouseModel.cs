using ShadowFlareRemake.UI.ItemsGrid;

namespace ShadowFlareRemake.UI.Warehouse
{
    public class WarehouseModel : Model
    {
        public ItemsGridModel WarehouseItemsGridModel { get; private set; }
        public bool IsWarehouseActive { get; private set; }

        public WarehouseModel() { }
    }
}
