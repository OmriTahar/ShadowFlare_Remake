using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogModel : Model
    {
        public Vector3 DialogBubblePosition { get; private set; }
        public string CurrentDialogText { get; private set; }
        public bool IsBubbleActive { get; private set; }
        public bool IsAnswersActive { get; private set; }

        public DialogModel() { }

        public void SetDialogBubblePosition(Vector3 pos)
        {
            DialogBubblePosition = pos;
            Changed();
        }

        public void SetIsDialogBubbleActive(bool isActive)
        {
            IsBubbleActive = isActive;
            Changed();
        }

        public void SetDialogText(string text)
        {
            CurrentDialogText = text;
            Changed();
        }

        public void SetIsAnswersActive(bool isActive)
        {
            IsAnswersActive = isActive;
            Changed();
        }
    }
}
