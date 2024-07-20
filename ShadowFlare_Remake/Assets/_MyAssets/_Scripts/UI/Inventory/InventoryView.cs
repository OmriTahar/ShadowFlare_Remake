using ShadowFlareRemake.Loot;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI.Inventory
{
    public class InventoryView : UIView<InventoryModel>
    {
        public event Action<LootModel, Vector2Int> OnTileHovered;
        public event Action<ItemsGridModel, Vector2Int, LootModel> OnTileClicked;
        public event Action<ItemsGridModel, bool> OnCursorChangedHoverOverGrid;

        [Header("References")]
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _equippedWeightText;
        [SerializeField] private Slider _equippedWeightSlider;
        [SerializeField] private Image _equippedWeightSlider_FillImage;

        [Header("Items Grids")]
        [SerializeField] private ItemsGridView _talismanItemsGridView;
        [SerializeField] private ItemsGridView _weaponItemsGridView;
        [SerializeField] private ItemsGridView _shieldItemsGridlView;
        [SerializeField] private ItemsGridView _helmetItemsGridView;
        [SerializeField] private ItemsGridView _armorItemsGridView;
        [SerializeField] private ItemsGridView _bootsItemsGridView;
        [SerializeField] private ItemsGridView _carryItemsGridView;
        [SerializeField] private ItemsGridView _quickItemsGridView;

        [Header("Settings")]
        [SerializeField] private Color _equippedWeightSlider_ValidColor;
        [SerializeField] private Color _equippedWeightSlider_OverWeightColor;

        private const float _sliderLerpDuration = 0.75f;

        private List<ItemsGridView> _itemsGridViews = new();
        private Coroutine _lastEquippedWeightCoroutine;
        private float _lastSeenEquippedWeight = 0f;

        #region View Overrides

        protected override void Initialize()
        {
            InitItemsGridViewsList();
            RegisterEvents();
            SetItemsGridModels();
        }

        protected override void ModelChanged()
        {
            HandleEquippedWeight();
            SetGoldAmount();
            SetIsActive();
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        #endregion

        #region Initialization

        private void InitItemsGridViewsList()
        {
            _itemsGridViews.Add(_talismanItemsGridView);
            _itemsGridViews.Add(_weaponItemsGridView);
            _itemsGridViews.Add(_shieldItemsGridlView);
            _itemsGridViews.Add(_helmetItemsGridView);
            _itemsGridViews.Add(_armorItemsGridView);
            _itemsGridViews.Add(_bootsItemsGridView);
            _itemsGridViews.Add(_carryItemsGridView);
            _itemsGridViews.Add(_quickItemsGridView);
        }

        private void SetItemsGridModels()
        {
            _talismanItemsGridView.SetModel(Model.TalismanItemsGridModel);
            _weaponItemsGridView.SetModel(Model.WeaponItemsGridModel);
            _shieldItemsGridlView.SetModel(Model.ShieldItemsGridModel);
            _helmetItemsGridView.SetModel(Model.HelmetItemsGridModel);
            _armorItemsGridView.SetModel(Model.ArmorItemsGridModel);
            _bootsItemsGridView.SetModel(Model.BootsItemsGridModel);
            _carryItemsGridView.SetModel(Model.CarryItemsGridModel);
            _quickItemsGridView.SetModel(Model.QuickItemsGridModel);
        }

        #endregion

        #region Meat & Potatos

        private void InvokeCursorChangedHoverOverGrid(ItemsGridModel itemsGridModel, bool isCursorOn)
        {
            OnCursorChangedHoverOverGrid?.Invoke(itemsGridModel, isCursorOn);
        }

        public void InvokeTileHovered(LootModel lootModel, Vector2Int index)
        {
            OnTileHovered?.Invoke(lootModel, index);
        }

        public void InvokeTileClicked(ItemsGridModel itemsGridModel, Vector2Int tileIndex, LootModel lootModel)
        {
            OnTileClicked?.Invoke(itemsGridModel, tileIndex, lootModel);
        }

        private void SetGoldAmount()
        {
            _goldText.text = Model.GoldAmount.ToString();
        }

        private void HandleEquippedWeight()
        {
            if(_lastEquippedWeightCoroutine != null)
            {
                StopCoroutine(_lastEquippedWeightCoroutine);
            }

            _equippedWeightSlider.maxValue = Model.Strength;
            _equippedWeightText.text = Model.EquippedWeight.ToString();

            _equippedWeightSlider.value = _lastSeenEquippedWeight;
            _lastEquippedWeightCoroutine = StartCoroutine(HandleLerpWeightSliderValueAndSetWeightColors(_equippedWeightSlider, Model.EquippedWeight));
        }

        public IEnumerator HandleLerpWeightSliderValueAndSetWeightColors(Slider slider, float targetValue)
        {
            float startValue = slider.value;
            float timeElapsed = 0f;

            bool isLastSeenOverWeight = _lastSeenEquippedWeight > Model.EquippedWeight;
            bool isCurrentlyOverWeight = Model.EquippedWeight > Model.Strength;
            bool isSliderValueDirectionIsUp = startValue < targetValue;

            SetEquippedWeightTextAndSliderColors(isLastSeenOverWeight, isCurrentlyOverWeight, isSliderValueDirectionIsUp);

            while(timeElapsed < _sliderLerpDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpProgress = timeElapsed / _sliderLerpDuration;
                slider.value = Mathf.Lerp(startValue, targetValue, lerpProgress);
                _lastSeenEquippedWeight = slider.value;
                yield return null;
            }

            SetEquippedWeightTextAndSliderColors(isLastSeenOverWeight, isCurrentlyOverWeight, isSliderValueDirectionIsUp);

            slider.value = targetValue;
            _lastSeenEquippedWeight = slider.value;
        }

        private void SetEquippedWeightTextAndSliderColors(bool isLastSeenOverWeight, bool isCurrentlyOverWeight, bool isSliderValueDirectionIsUp)
        {
            if(isLastSeenOverWeight && !isCurrentlyOverWeight && !isSliderValueDirectionIsUp)
            {
                _equippedWeightSlider_FillImage.color = _equippedWeightSlider_ValidColor;
                _equippedWeightText.color = _equippedWeightSlider_ValidColor;
            }

            else if(!isLastSeenOverWeight && isCurrentlyOverWeight && isSliderValueDirectionIsUp)
            {
                _equippedWeightSlider_FillImage.color = _equippedWeightSlider_OverWeightColor;
                _equippedWeightText.color = _equippedWeightSlider_OverWeightColor;
            }
        }

        private void SetIsActive()
        {
            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            foreach(var view in _itemsGridViews)
            {
                view.OnCursorChangedHoverOverGrid += InvokeCursorChangedHoverOverGrid;
                view.OnTileHovered += InvokeTileHovered;
                view.OnTileClicked += InvokeTileClicked;
            }
        }

        private void DeregisterEvents()
        {
            foreach(var view in _itemsGridViews)
            {
                view.OnCursorChangedHoverOverGrid -= InvokeCursorChangedHoverOverGrid;
                view.OnTileHovered -= InvokeTileHovered;
                view.OnTileClicked -= InvokeTileClicked;
            }
        }

        #endregion
    }
}
