
namespace ShadowFlareRemake.Items {
    public class HealthPotion : Potion {

        public HealthPotion() {

            Name = "Health Potion";
            Weight = 0.5f;
            Type = ItemType.Consumbale;

            HealthRestore = 50;
            ManaRestore = 0;
        }
    }
}

