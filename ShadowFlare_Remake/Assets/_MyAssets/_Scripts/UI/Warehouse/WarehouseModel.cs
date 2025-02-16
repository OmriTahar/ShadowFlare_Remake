using ShadowFlareRemake.UI.ItemsGrid;

namespace ShadowFlareRemake.UI.Warehouse
{
    public class WarehouseModel : Model
    {
        public ItemsGridModel WarehouseItemsGridModel { get; private set; }
        public bool IsPanelActive { get; private set; }
        
        public WarehouseModel() { }

        public void SetIsActive(bool isActive)
        {
            if(IsPanelActive == isActive)
                return;

            IsPanelActive = isActive;
            Changed();
        }
    }
}
