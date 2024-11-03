using ShadowFlareRemake.Npc;
using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogModel : Model
    {
        public NpcView CurrentNpc { get; private set; }
        public Vector3 DialogBubblePosition { get; private set; }
        public DialogTextData CurrentDialogTextData { get; private set; }
        public bool IsBubbleActive { get; private set; }
        public bool IsAnswersActive { get; private set; }

        public int CurrentAnswerId = -1;

        public DialogModel() { }

        public void SetCurrentNpc(NpcView npc)
        {
            CurrentNpc = npc;
            Changed();
        }

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

        public void SetDialogTextData(DialogTextData data)
        {
            CurrentDialogTextData = data;
            Changed();
        }

        public void SetIsAnswersActive(bool isActive)
        {
            IsAnswersActive = isActive;
            Changed();
        }

        public void SetAnswerId(int id)
        {
            CurrentAnswerId = id;
            Changed();
        }
    }
}
