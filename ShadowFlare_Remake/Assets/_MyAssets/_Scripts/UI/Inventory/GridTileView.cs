using ShadowFlareRemake.Loot;
using System;
using UnityEngine;

namespace ShadowFlareRemake.UI.Inventory
{
    public class GridTileView : View<GridTileModel>
    {
        public event Action<Vector2Int> OnTileClicked;

        [Header("References")]
        [SerializeField] private LootView LootView;
        [field: SerializeField] public Vector2Int Index { get; private set; }

        protected override void ModelChanged()
        {
            SetLootModel();
        }

        private void SetLootModel()
        {
            if(Model.LootModel == null)
            {
                LootView.gameObject.SetActive(false);
                return;
            }

            LootView.gameObject.SetActive(true);
            LootView.SetModel(Model.LootModel);
        }

        public void TileClicked()
        {
            OnTileClicked?.Invoke(Index);
        }
    }
}
