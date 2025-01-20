using System;

namespace ShadowFlareRemake.Npc
{
    [Serializable]
    public class DialogTextData
    {
        public string Title;
        public int Id;
        public string DialogText;
        public bool IsQuestionText;
        public bool IsFinalText;
        public DialogAnswerData[] Answers;
    }
}
