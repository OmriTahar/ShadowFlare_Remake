using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI.Inventory
{
    public class GridTileView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector2Int, bool> OnTileHovered;
        public event Action<Vector2Int> OnTileClicked;

        [field: SerializeField] public Vector2Int Index { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnTileHovered?.Invoke(Index, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnTileHovered?.Invoke(Index, false);
        }

        public void TileClicked()
        {
            OnTileClicked?.Invoke(Index);
        }
    }
}
