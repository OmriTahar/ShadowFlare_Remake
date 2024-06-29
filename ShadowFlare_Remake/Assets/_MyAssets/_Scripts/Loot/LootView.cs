using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShadowFlareRemake.Loot
{
    public class LootView : View<LootModel>
    {
        public event Action OnLootViewClicked;

        [Header("Refernces")]
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Animator _animator;

        private readonly int _dropAnimHash = Animator.StringToHash("Drop");
        private const int _sizeMultiplier = 58;
        private const int _posHelper = 35;

        private string _name;
        private Sprite _sprite;
        private int _width;
        private int _height;

        #region View Overrides

        protected override void ModelChanged()
        {
            if(Model == null)
                return;

            SetData();
            SetNameText();
            HandleSetSprite();
            HandleAnimation();
        }

        #endregion

        #region Initialization

        public LootModel GetLootModel()
        {
            return Model;
        }

        #endregion

        #region Meat & Potatos

        private void SetData()
        {
            if(Model.LootData == null)
            {
                print("Loot Data is null!");
                return;
            }

            _name = Model.LootData.Name;
            _sprite = Model.LootData.Sprite;
            _width = Model.LootData.Width;
            _height = Model.LootData.Height;
        }

        private void SetNameText()
        {
            if(_nameText != null && !string.IsNullOrEmpty(_name))
                _nameText.text = _name;
        }

        private void HandleSetSprite()
        {
            if(_image == null)
                return;

            SetSprite();
            SetSpriteSize();
        }

        private void SetSprite()
        {
            _image.sprite = _sprite;
            _image.preserveAspect = true;
        }

        private void SetSpriteSize()
        {
            _rect.sizeDelta = new Vector2(_width * _sizeMultiplier, _height * _sizeMultiplier);

            if(Model.IsSingleTile)
            {
                _rect.localPosition = new Vector2(0, 0);
                return;
            }

            var x = (_width - 1) * _posHelper;
            var y = (_height - 1) * -_posHelper;
            _rect.localPosition = new Vector2(x, y);
        }

        public void UILootViewClicked()  // Called from a UI button clicked event
        {
            OnLootViewClicked?.Invoke();
        }

        private void HandleAnimation()
        {
            if(_animator == null)
                return;

            if(Model.IsInvokeDropAnimation)
            {
                _animator.SetTrigger(_dropAnimHash);
            }
        }

        #endregion
    }
}


