using ShadowFlareRemake.Loot;
using ShadowFlareRemake.UI.ItemsGrid;
using System.Collections.Generic;

namespace ShadowFlareRemake.UI.Warehouse
{
    public class WarehouseModel : Model
    {
        public ItemsGridModel WarehouseItemsGridModel { get; private set; }
        public bool IsPanelActive { get; private set; }
        
        public WarehouseModel() 
        {
            WarehouseItemsGridModel = new ItemsGridModel(ItemsGridType.Carry, 10, 10, new List<LootType>() { LootType.All }, false);
        }

        public void SetIsActive(bool isActive)
        {
            if(IsPanelActive == isActive)
                return;

            IsPanelActive = isActive;
            Changed();
        }
    }
}
