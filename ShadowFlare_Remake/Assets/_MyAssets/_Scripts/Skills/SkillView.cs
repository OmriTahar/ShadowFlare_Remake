using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.Skills
{
    public class SkillView : View<SkillModel>
    {
        [Header("References")]
        [SerializeField] private Image _image;

        protected override void ModelChanged()
        {
            SetSprite();
        }

        private void SetSprite()
        {
            _image.sprite = Model.SkillData.Sprite;
        }
    }
}
