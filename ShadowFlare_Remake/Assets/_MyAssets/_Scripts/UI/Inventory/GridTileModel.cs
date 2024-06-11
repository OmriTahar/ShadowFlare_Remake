using ShadowFlareRemake.Loot;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class GridTileModel : Model
    {
        public LootModel LootModel { get; private set; }
        public Vector2Int TileIndex { get; private set; }
        public bool IsSingleTile { get; private set; }

        public GridTileModel(Vector2Int tileIndex, bool isSingleTile)
        {
            TileIndex = tileIndex;
            IsSingleTile = isSingleTile;
        }

        public void SetLootModel(LootModel lootModel)
        {
            LootModel = lootModel;

            if (lootModel != null)
            {
                LootModel.SetIsSingleTile(IsSingleTile);
            }

            Changed();
        }
    }
}
