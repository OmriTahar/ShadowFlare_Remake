using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI
{
    public class LootInfoTooltipModel : Model
    {
        public LootModel LootModel { get; private set; }
        public bool IsActive { get; private set; }

        public LootInfoTooltipModel() { }

        public void SetLootModel(LootModel lootModel)
        {
            if(LootModel == lootModel)
                return;

            LootModel = lootModel;
            Changed();
        }

        public void SetIsActive(bool isActive)
        {
            if(IsActive == isActive)
                return;

            IsActive = isActive;
            Changed();
        }

        public void InvokeChanged()
        {
            Changed();
        }
    }
}
