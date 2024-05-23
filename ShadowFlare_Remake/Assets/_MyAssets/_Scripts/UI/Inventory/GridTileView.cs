using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory {
    public class GridTileView : MonoBehaviour {

        public event Action<Vector2Int> OnTileClicked;

        [field: SerializeField] public Vector2Int Index { get; private set; }

        public void TileClicked() {

            OnTileClicked?.Invoke(Index);
        }
    }
}
