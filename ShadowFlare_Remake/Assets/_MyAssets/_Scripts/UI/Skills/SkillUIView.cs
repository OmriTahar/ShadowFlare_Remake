using ShadowFlareRemake.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI.Skills
{
    public class SkillUIView : View<SkillModel>
    {
        [Header("References")]
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Image _image;

        [Header("Settings")]
        [SerializeField] private float _selectedSize = 80;
        [SerializeField] private float _idleSize = 60;

        protected override void ModelChanged()
        {
            SetSprite();
            SetSpriteSize();
        }

        private void SetSprite()
        {
            if (_image == null || Model.SkillData == null || Model.SkillData.Sprite == null)
                return;

            _image.enabled = true;
            _image.sprite = Model.SkillData.Sprite;
        }

        private void SetSpriteSize()
        {
            var size = Model.IsSelected? _selectedSize : _idleSize;
            _rect.sizeDelta = new Vector2(size, size);
        }
    }
}
