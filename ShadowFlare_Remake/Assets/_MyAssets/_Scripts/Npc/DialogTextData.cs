using System;
using UnityEngine;

namespace ShadowFlareRemake.Npc
{
    [Serializable]
    public class DialogTextData
    {
        public int Id;
        public string Title;
        public string DialogText;
        public bool IsFinalText;
        public DialogAnswerData[] Answers;

        //[Space(10)]
        //public string FirstAnswer_Title;
        //public int FirstAnswer_NextTextId = -1;
        //[Space(5)]
        //public string SecondAnswer_Title;
        //public int SecondAnswer_NextTextId = -1;
        //[Space(5)]
        //public string ThirdAnswer_Title;
        //public int ThirdAnswer_NextTextId = -1;
    }
}
