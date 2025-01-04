using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI.NameBubble
{
    public class EntityNameView : View<EntityNameModel>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Slider _nameSlider;
        [SerializeField] private Transform _nameBubbleTransform;
        [SerializeField] private RectTransform _nameBubbleRectTransform;

        [Header("Settings")]
        [SerializeField] private Vector2 _smallSliderSize;
        [SerializeField] private Vector2 _mediumSliderSize;
        [SerializeField] private Vector2 _largeSliderSize;

        private Transform _currentEntityTransform;

        private void Update()
        {
            if(_currentEntityTransform == null)
                return;

            SetNameUiPosition();
        }

        protected override void ModelChanged()
        {
            SetSliderSize();
            SetValues();
            SetNameUiPosition();
            SetIsActive();
        }

        private void SetSliderSize()
        {
            switch(Model.EvolutionLevel)
            {
                case 1:
                    _nameBubbleRectTransform.sizeDelta = new Vector2(_smallSliderSize.x, _smallSliderSize.y);
                    break;
                case 2:
                    _nameBubbleRectTransform.sizeDelta = new Vector2(_mediumSliderSize.x, _mediumSliderSize.y);
                    break;
                case 3:
                    _nameBubbleRectTransform.sizeDelta = new Vector2(_largeSliderSize.x, _largeSliderSize.y);
                    break;
                default:
                    _nameBubbleRectTransform.sizeDelta = new Vector2(_smallSliderSize.x, _smallSliderSize.y);
                    break;
            }
        }

        private void SetValues()
        {
            if(_nameText != null)
                _nameText.text = Model.Name;

            if(Model.EntityType != EntityType.Enemy)
                return;

            _nameSlider.maxValue = Model.MaxHP;
            _nameSlider.value = Model.CurrentHP;
            _currentEntityTransform = Model.CurrentEntityTransform;
        }


        private void SetNameUiPosition()
        {
            var pos = GetNameUiPosition();
            _nameBubbleTransform.position = GetNameUiPosition();
        }

        private Vector3 GetNameUiPosition()
        {
            if(_currentEntityTransform == null)
                return Vector3.zero;

            var screenPoint = Camera.main.WorldToScreenPoint(Model.CurrentEntityTransform.position);
            var bubbleOffset = Model.UiOffest * Model.ScaleMultiplier;
            return new Vector3(screenPoint.x, screenPoint.y + bubbleOffset, screenPoint.z);
        }

        public void SetIsActive()
        {
            _nameSlider.gameObject.SetActive(Model.IsActive);
        }
    }
}
