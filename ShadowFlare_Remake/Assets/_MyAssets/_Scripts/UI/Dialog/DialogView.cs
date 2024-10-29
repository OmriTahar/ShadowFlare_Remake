using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogView : UIView<DialogModel>
    {
        [Header("References")]
        [SerializeField] private Transform _speechBubbleHolder;

        protected override void ModelChanged()
        {
            _speechBubbleHolder.gameObject.SetActive(Model.IsBubbleActive);
            _speechBubbleHolder.position = Model.SpeechBubblePosition;
        }
    }
}
