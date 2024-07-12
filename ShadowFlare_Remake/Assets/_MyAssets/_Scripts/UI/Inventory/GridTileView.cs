using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI.Inventory
{
    public class GridTileView : UIView<GridTileModel>, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector2Int, bool> OnTileHovered;
        public event Action<Vector2Int> OnTileClicked;

        [field: SerializeField] public Vector2Int Index { get; private set; }

        [Header("References")]
        [SerializeField] private LootView _lootView;

        #region View Overrides

        protected override void Initialize()
        {
            _lootView.OnLootViewClicked += InvokeTileClicked;
        }

        protected override void ModelChanged()
        {
            SetLootModel();
        }

        protected override void Clean()
        {
            _lootView.OnLootViewClicked -= InvokeTileClicked;
        }

        #endregion

        #region Meat & Potatos

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

        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnTileHovered?.Invoke(Index, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            OnTileHovered?.Invoke(Index, false);
        }

        public void InvokeTileClicked()
        {
            OnTileClicked?.Invoke(Index);
        }

        #endregion
    }
}
