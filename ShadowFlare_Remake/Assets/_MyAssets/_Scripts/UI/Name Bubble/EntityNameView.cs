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
        }

        public void SetIsActive()
        {
            _nameSlider.gameObject.SetActive(Model.IsActive);
        }
    }
}
