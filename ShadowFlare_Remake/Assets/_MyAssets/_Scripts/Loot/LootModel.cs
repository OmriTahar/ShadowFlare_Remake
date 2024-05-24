using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootModel : Model
    {
        public  Loot_ScriptableObject LootData { get; private set; }

        public Color Color { get; private set; }
        public Color HighlightColor { get; private set; }
        
        public LootModel(Loot_ScriptableObject lootData)
        {
            LootData = lootData;
            Color = new Color(lootData.Color.r, lootData.Color.g, lootData.Color.b, 1);
            HighlightColor = new Color(Color.r + 0.2f, Color.g + 0.2f, Color.b + 0.2f, 1);
        }
    }
}
