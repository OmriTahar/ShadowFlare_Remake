using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.Skills
{
    public class SkillView : View<SkillModel>
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
            
            _image.sprite = Model.SkillData.Sprite;
        }

        private void SetSpriteSize()
        {
            var size = Model.IsSelected? _selectedSize : _idleSize;
            _rect.sizeDelta = new Vector2(size, size);
        }
    }
}
