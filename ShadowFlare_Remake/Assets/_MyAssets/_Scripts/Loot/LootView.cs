using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.Loot
{
    public class LootView : View<LootModel>
    {
        public event Action OnLootViewClicked;
        public string Name { get => Model.LootData.Name; }
        public int Amount { get => Model.Amount; }
        public LootCategory LootCategory { get => Model.LootCategory; }

        [Header("Refernces")]
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Image _image;
        [SerializeField] private Animator _animator;

        private readonly int _dropAnimHash = Animator.StringToHash("Drop");
        private const int _generalSizeMultiplier = 58;
        private const int _goldSizeMultiplier = 62;
        private const int _posHelper = 35;

        private const int _goldAmount_Tier1 = 3;
        private const int _goldAmount_Tier2 = 100;
        private const int _goldAmount_Tier3 = 500;
        private const int _goldAmount_Tier4 = 1000;
        private const int _goldAmount_Tier5 = 9999;

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
            var size = Model.LootCategory == LootCategory.Gold ? _goldSizeMultiplier : _generalSizeMultiplier;
            _rect.sizeDelta = new Vector2(_width * size, _height * size);
        }

        private void SetSpriteLocalPosition()
        {
            if(!Model.IsAllowedToSetLocalPosition)
                return;

            if(Model.IsSingleTile)
            {
                _rect.localPosition = new Vector2(0, 0);
                return;
            }

            var x = (_width - 1) * _posHelper;
            var y = (_height - 1) * -_posHelper;
            _rect.localPosition = new Vector2(x, y);
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

            else if(amount >= _goldAmount_Tier1 && amount < _goldAmount_Tier2)
            {
                _image.sprite = data.GoldImage_2_to_99;
            }

            else if(amount >= _goldAmount_Tier2 && amount < _goldAmount_Tier3)
            {
                _image.sprite = data.GoldImage_100_to_499;
            }

            else if(amount >= _goldAmount_Tier3 && amount < _goldAmount_Tier4)
            {
                _image.sprite = data.GoldImage_500_to_999;
            }

            else if(amount >= _goldAmount_Tier4 && amount < _goldAmount_Tier5)
            {
                _image.sprite = data.GoldImage_1000_to_9999;
            }

            else
                _image.sprite = data.GoldImage_10000;
        }

        #endregion
    }
}


