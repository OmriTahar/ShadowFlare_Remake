using UnityEngine;

namespace ShadowFlareRemake.Items {

    public abstract class Potion : MonoBehaviour, I_Item {
        public string Name { get; set; }
        public float Weight { get; set; }
        public ItemType Type { get; set; }

        protected int HealthRestore;
        protected int ManaRestore;

        public virtual void Use(Unit unit) {
            if(unit != null) {
                unit.HealHP(HealthRestore);
            }
        }
    }
}

