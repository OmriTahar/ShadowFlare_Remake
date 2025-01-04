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

        private Transform _currentEntityTransform;

        private void Update()
        {
            if(_currentEntityTransform == null)
                return;

            SetNameUiPosition();
        }

        protected override void ModelChanged()
        {
            SetValues();
            SetIsActive();
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
            SetNameUiPosition();
        }

        public void SetIsActive()
        {
            _nameSlider.gameObject.SetActive(Model.IsActive);
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
    }
}
