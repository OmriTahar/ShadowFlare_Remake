using ShadowFlareRemake.Loot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI.Highlightables
{
    public class HighlightableNameView : View<HighlightableNameModel>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Slider _nameSlider;
        [SerializeField] private Image _nameSliderFill;

        [Header("Settings")]
        [SerializeField] private Vector2 _smallSliderSize;
        [SerializeField] private Vector2 _mediumSliderSize;
        [SerializeField] private Vector2 _largeSliderSize;

        [Header("Colors")]
        [SerializeField] private Color _nameBG_NpcColor;
        [SerializeField] private Color _nameBG_EnemyColor;
        [SerializeField] private Color _nameBG_EquipmentColor;
        [SerializeField] private Color _nameBG_GoldColor;
        [SerializeField] private Color _nameBG_PotionsColor;

        private const int _one = 1;

        private Transform _currentEntityTransform;
        private RectTransform _nameSliderRectTrans;

        private void Update()
        {
            if(_currentEntityTransform == null)
                return;

            SetNameUiPosition();
        }

        protected override void Initialize()
        {
            CacheReferences();
        }

        protected override void ModelChanged()
        {
            SetValues();
            SetSliderSize();
            SetBackgroundColor();
            SetNameUiPosition();
            SetIsActive();
        }

        private void CacheReferences()
        {
            _nameSliderRectTrans = _nameSlider.GetComponent<RectTransform>();
        }

        private void SetValues()
        {
            var name = Model.Name;

            if(IsGold())
                name = $"{Model.GoldAmount} {Model.Name}";

            _nameText.text = name;
            _currentEntityTransform = Model.CurrentEntityTransform;

            if(Model.EntityType != EntityType.Enemy)
            {
                _nameSlider.maxValue = _one;
                _nameSlider.value = _one;
                return;
            }

            _nameSlider.maxValue = Model.MaxHP;
            _nameSlider.value = Model.CurrentHP;
        }

        private bool IsGold()
        {
            return Model.EntityType == EntityType.Loot && Model.LootCategory == LootCategory.Gold;
        }

        private void SetSliderSize()
        {
            switch(Model.EvolutionLevel)
            {
                case 1:
                    _nameSliderRectTrans.sizeDelta = _smallSliderSize;
                    break;
                case 2:
                    _nameSliderRectTrans.sizeDelta = _mediumSliderSize;
                    break;
                case 3:
                    _nameSliderRectTrans.sizeDelta = _largeSliderSize;
                    break;
                default:
                    _nameSliderRectTrans.sizeDelta = _mediumSliderSize;
                    break;
            }
        }

        private void SetBackgroundColor()
        {
            switch(Model.EntityType)
            {
                case EntityType.Npc:
                    _nameSliderFill.color = _nameBG_NpcColor;
                    break;

                case EntityType.Enemy:
                    _nameSliderFill.color = _nameBG_EnemyColor;
                    break;

                case EntityType.Loot:

                    switch(Model.LootCategory)
                    {
                        case LootCategory.Equipment:
                            _nameSliderFill.color = _nameBG_EquipmentColor;
                            break;

                        case LootCategory.Potion:
                            _nameSliderFill.color = _nameBG_PotionsColor;
                            break;

                        case LootCategory.Gold:
                            _nameSliderFill.color = _nameBG_GoldColor;
                            break;
                    }

                    break;

                default:
                    _nameSliderFill.color = _nameBG_NpcColor;
                    break;
            }
        }

        private void SetNameUiPosition()
        {
            _nameSliderRectTrans.position = GetNameUiPosition();
        }

        private Vector3 GetNameUiPosition()
        {
            if(_currentEntityTransform == null)
            {
                return Vector3.zero;
            }

            var screenPoint = Camera.main.WorldToScreenPoint(_currentEntityTransform.position);
            var bubbleOffset = Model.UiOffest * Model.ScaleMultiplier;
            return new Vector3(screenPoint.x, screenPoint.y + bubbleOffset, screenPoint.z);
        }

        public void SetIsActive()
        {
            _nameSlider.gameObject.SetActive(Model.IsActive);
        }
    }
}