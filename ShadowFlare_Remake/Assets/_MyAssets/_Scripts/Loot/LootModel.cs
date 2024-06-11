using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootModel : Model
    {
        public  Loot_ScriptableObject LootData { get; private set; }

        public Color Color { get; private set; }
        public Color HighlightColor { get; private set; }
        public bool IsSingleTile { get; private set; } = false;

        private const float _highlightAdder = 0.2f;
        private const float _alpha = 1;

        public LootModel(Loot_ScriptableObject lootData)
        {
            LootData = lootData;

            Color = new Color(lootData.Color.r, lootData.Color.g, lootData.Color.b, 1);
            HighlightColor = new Color(Color.r + _highlightAdder, Color.g + _highlightAdder, Color.b + _highlightAdder, _alpha);
        }

        public void SetIsSingleTile(bool isSingleTile)
        {
            IsSingleTile = isSingleTile;
        }
    }
}
