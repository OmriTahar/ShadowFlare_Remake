
namespace ShadowFlareRemake.Items {
    public interface I_Item {

        public string Name { get; set; }
        public float Weight { get; set; }
        public ItemType Type { get; set; }

        public void Use(Unit unit);
    }
}

//public enum ItemType {
//    Consumbale,
//    Quest
//}
