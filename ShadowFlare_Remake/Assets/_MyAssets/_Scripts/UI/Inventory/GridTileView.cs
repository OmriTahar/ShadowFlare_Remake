using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI.Inventory
{
    public class GridTileView : View<GridTileModel>, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector2Int> OnTileClicked;
        public event Action<Vector2Int> OnTileHovered;

        [field: SerializeField] public Vector2Int Index { get; private set; }

        [Header("References")]
        [SerializeField] private LootView _lootView;

        protected override void Initialize()
        {
            _lootView.OnLootViewClicked += TileClicked;
        }

        protected override void Clean()
        {
            _lootView.OnLootViewClicked -= TileClicked;
        }

        protected override void ModelChanged()
        {
            SetLootModel();
        }

        private void SetLootModel()
        {
            if(Model.LootModel == null)
            {
                _lootView.gameObject.SetActive(false);
                return;
            }

            _lootView.gameObject.SetActive(true);
            _lootView.SetModel(Model.LootModel);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnTileHovered?.Invoke(Index);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnTileHovered?.Invoke(Index);
        }

        public void TileClicked()
        {
            OnTileClicked?.Invoke(Index);
        }
    }
}
