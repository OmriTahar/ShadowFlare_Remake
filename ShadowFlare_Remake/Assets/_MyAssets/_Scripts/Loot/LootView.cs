using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.Loot
{
    public class LootView : View<LootModel>
    {
        public event Action OnLootViewClicked;

        public string Name { get; private set; }
        public Sprite Sprite { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        [Header("Refernces")]
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;

        [Header("Settings")]
        [SerializeField] private int _spriteSizeMultiplier = 60;
        [SerializeField] private int _spritePosHelper = 40;

        protected override void ModelChanged()
        {
            SetData();
            SetName();
            SetSprite();
        }

        public LootModel GetLootModel()
        {
            return Model;
        }
     
        private void SetData()
        {
            if(Model.LootData == null)
            {
                print("Loot Data is null!");
                return;
            }

            Name = Model.LootData.Name;
            Sprite = Model.LootData.Sprite;
            Width = Model.LootData.Width;
            Height = Model.LootData.Height;
        }

        private void SetName()
        {
            if(_nameText != null)
                _nameText.text = Name;
        }

        private void SetSprite()
        {
            if(_image == null)
                return;

            _image.sprite = Sprite;
            _image.preserveAspect = true;

            _rect.sizeDelta = new Vector2(Width * _spriteSizeMultiplier, Height * _spriteSizeMultiplier);

            var x = (Width - 1) * _spritePosHelper;
            var y = (Height - 1) * -_spritePosHelper;
            _rect.localPosition = new Vector2(x,y);
        }

        public void UILootViewClicked()
        {
            OnLootViewClicked?.Invoke();
        }
    }
}


