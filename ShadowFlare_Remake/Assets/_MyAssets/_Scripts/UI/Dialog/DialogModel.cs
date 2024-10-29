using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogModel : Model
    {
        public Vector3 SpeechBubblePosition { get; private set; }
        public bool IsBubbleActive { get; private set; }
        public string Text { get; private set; }

        public DialogModel() { }

        public void SetSpeechBubblePosition(Vector3 pos)
        {
            SpeechBubblePosition = pos;
            Changed();
        }

        public void SetIsBubbleActive(bool isActive)
        {
            IsBubbleActive = isActive;
            Changed();
        }

        public void SetText(string text)
        {
            Text = text;
            Changed();
        }
    }
}
