using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ShadowFlareRemake.Loot;

namespace ShadowFlareRemake.UI.Inventory
{
    public class GridTileView : View<GridTileModel>, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector2Int> OnTileHovered;
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            InvokeTileHovered();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //InvokeTileHovered();
        }

        public void InvokeTileHovered()
        {
            OnTileHovered?.Invoke(Index);
        }

        public void InvokeTileClicked()
        {
            OnTileClicked?.Invoke(Index);
        }

        #endregion
    }
}
