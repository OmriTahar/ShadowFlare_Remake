using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory {
    public class GridTileModel : Model {

        public Vector2 Index { get; private set; }
    
        public GridTileModel(Vector2Int index) { 

            Index = index;
        }
    }
}
