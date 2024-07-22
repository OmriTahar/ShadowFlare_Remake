using ShadowFlareRemake.Enums;
using System;
using TMPro;
using UnityEngine;
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
        [SerializeField] private Image _nameBackgroundImage;

        [Header("Settings")]
        [SerializeField] private Color _nameBG_EquipmentColor;
        [SerializeField] private Color _nameBG_GoldColor;
        [SerializeField] private Color _nameBG_PotionsColor;

        private readonly int _dropAnimHash = Animator.StringToHash("Drop");
        private const int _sizeMultiplier = 62;
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
            SetNameBackgroundColor();
            HandleSprite();
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
            {
                if(Model.LootCategory == Enums.LootCategory.Gold)
                {
                    _nameText.text = $"{Model.Amount} {_name}";
                    return;
                }

                _nameText.text = _name;
            }
        }

        private void SetNameBackgroundColor()
        {
            if(_nameBackgroundImage == null)
                return;

            switch(Model.LootCategory)
            {
                case LootCategory.Equipment:
                    _nameBackgroundImage.color = _nameBG_EquipmentColor;
                    break;

                case LootCategory.Potion:
                    _nameBackgroundImage.color = _nameBG_PotionsColor;
                    break;

                case LootCategory.Gold:
                    _nameBackgroundImage.color = _nameBG_GoldColor;
                    break;
            }
        }

        private void HandleSprite()
        {
            if(_image == null)
                return;

            SetSprite();
            SetSpriteSize();
            SetSpriteLocalPosition();
        }

        private void SetSprite()
        {
            _image.preserveAspect = true;

            if(Model.LootCategory == LootCategory.Gold)
            {
                SetGoldSprite();
                return;
            }

            _image.sprite = _sprite;

        }

        private void SetSpriteSize()
        {
            _rect.sizeDelta = new Vector2(_width * _sizeMultiplier, _height * _sizeMultiplier);
        }

        private void SetSpriteLocalPosition()
        {
            if(!Model.IsAllowedToSetLocalPosition)
                return;

            if(Model.IsSingleTile)
            {
                _rect.localPosition = new Vector2(0, 0);
                print("Local Position: " + _rect.localPosition);
                return;
            }

            var x = (_width - 1) * _posHelper;
            var y = (_height - 1) * -_posHelper;
            _rect.localPosition = new Vector2(x, y);
            print("Local Position: " + _rect.localPosition);
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

        public void UILootViewClicked()  // Called from a UI button clicked event
        {
            OnLootViewClicked?.Invoke();
        }

        private void SetGoldSprite()
        {
            var data = Model.LootData as GoldData_ScriptableObject;
            var amount = Model.Amount;

            if(amount < 3)
            {
                _image.sprite = data.GoldImage_1;
            }

            else if(amount >= 3 && amount < 100)
            {
                _image.sprite = data.GoldImage_2_to_99;
            }

            else if(amount >= 100 && amount < 500)
            {
                _image.sprite = data.GoldImage_100_to_499;
            }

            else if(amount >= 500 && amount < 1000)
            {
                _image.sprite = data.GoldImage_500_to_999;
            }

            else if(amount >= 1000 && amount < 10000)
            {
                _image.sprite = data.GoldImage_1000_to_9999;
            }

            else
                _image.sprite = data.GoldImage_10000;
        }

        #endregion
    }
}


