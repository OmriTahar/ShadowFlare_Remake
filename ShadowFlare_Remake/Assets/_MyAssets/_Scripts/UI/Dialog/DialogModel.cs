using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogModel : Model
    {
        public Vector3 SpeechBubblePosition { get; private set; }
        public string CurrentText { get; private set; }
        public bool IsBubbleActive { get; private set; }
        public bool IsAnswersActive { get; private set; }

        public DialogModel() { }

        public void SetTextBubblePosition(Vector3 pos)
        {
            SpeechBubblePosition = pos;
            Changed();
        }

        public void SetIsTextBubbleActive(bool isActive)
        {
            IsBubbleActive = isActive;
            Changed();
        }

        public void SetText(string text)
        {
            CurrentText = text;
            Changed();
        }

        public void SetIsAnswersActive(bool isActive)
        {
            IsAnswersActive = isActive;
            Changed();
        }
    }
}
