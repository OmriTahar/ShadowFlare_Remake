using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class GridTileView : MonoBehaviour
    {
        public event Action<Vector2Int, InventoryItem> OnTileClicked;

        [field: SerializeField] public Vector2Int Index { get; private set; }

        public void TileClicked()
        {
            if(transform.childCount <= 0)
            {
                OnTileClicked?.Invoke(Index, null);
                return;
            }

            var inventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();
            OnTileClicked?.Invoke(Index, inventoryItem);
        }
    }
}
