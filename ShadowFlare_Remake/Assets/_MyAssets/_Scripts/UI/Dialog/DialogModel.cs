using ShadowFlareRemake.Npc;
using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogModel : Model
    {
        public NpcBehaviour CurrentNpc { get; private set; }
        public Vector3 DialogBubblePosition { get; private set; }
        public DialogTextData CurrentDialogTextData { get; private set; }
        public bool IsBubbleActive { get; private set; }
        public bool IsQuestionText { get; private set; }
        public bool IsFinalText { get; private set; }
        public int CurrentDialogTextId { get; private set; }

        public DialogModel() { }

        public void SetCurrentNpc(NpcBehaviour npc, bool invokeChanged = true)
        {
            CurrentNpc = npc;

            if(invokeChanged)
                Changed();
        }

        public void SetDialogBubblePosition(Vector3 pos)
        {
            DialogBubblePosition = pos;
            Changed();
        }

        public void SetIsDialogBubbleActive(bool isActive, bool invokeChanged = true)
        {
            IsBubbleActive = isActive;

            if(invokeChanged)
                Changed();
        }

        public void SetDialogTextData(DialogTextData data)
        {
            CurrentDialogTextData = data;
            SetCurrentDialogTextId();
            SetIsQuestionText();
            SetIsFinalText();
            Changed();
        }

        private void SetIsQuestionText()
        {
            if(CurrentDialogTextData == null)
                return;

            IsQuestionText = CurrentDialogTextData.IsQuestionText;
        }

        private void SetIsFinalText()
        {
            if(CurrentDialogTextData == null)
                return;

            IsFinalText = CurrentDialogTextData.IsFinalText;
        }

        private void SetCurrentDialogTextId()
        {
            if(CurrentDialogTextData == null)
                return;

            CurrentDialogTextId = CurrentDialogTextData.Id;
        }

        #region Reset Functions

        public void ResetDialogModel()
        {
            bool invokeChanged = false;

            ResetTextId(invokeChanged);
            SetCurrentNpc(null, invokeChanged);
            ResetIsQuestionText(invokeChanged);
            ResetIsFinalText(invokeChanged);
            SetIsDialogBubbleActive(false, invokeChanged);

            Changed();
        }

        private void ResetIsQuestionText(bool invokeChanged = true)
        {
            IsQuestionText = false;

            if(invokeChanged)
                Changed();
        }

        private void ResetIsFinalText(bool invokeChanged = true)
        {
            IsFinalText = false;

            if(invokeChanged)
                Changed();
        }

        private void ResetTextId(bool invokeChanged = true)
        {
            if(CurrentNpc == null)
                return;

            CurrentNpc.ResetCurrentDialogTextId();

            if(invokeChanged)
                Changed();
        }

        #endregion
    }
}
