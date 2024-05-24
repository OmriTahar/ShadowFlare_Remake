using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.Loot
{
    public class LootView : View<LootModel>, IHighlightable
    {
        public bool IsHighlighted { get; private set; }

        public string Name { get; private set; }
        public Texture2D Icon { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        [Header("Refernces")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _nameHolder;
        [SerializeField] private TMP_Text _nameText;

        protected override void ModelChanged()
        {
            SetData();
            SetText();
        }

        private void SetData()
        {
            if(Model.LootData == null)
            {
                print("Loot Data is null!");
                return;
            }

            Name = Model.LootData.Name;
            Icon = Model.LootData.Icon;
            Width = Model.LootData.Width;
            Height = Model.LootData.Height;
        }

        private void SetText()
        {
            if(!string.IsNullOrEmpty(Name))
            {
                _nameText.text = Name;
            }
        }

        public void SetIsHighlighted(bool isHighlighted)
        {
            if(IsHighlighted == isHighlighted)
                return;

            IsHighlighted = isHighlighted;
            HandleIsHighlighted();
        }

        private void HandleIsHighlighted()
        {
            if (Model == null)
                return;

            _nameHolder.SetActive(IsHighlighted);
            _meshRenderer.material.color = IsHighlighted ? Model.HighlightColor : Model.Color;
        }

    }
}


