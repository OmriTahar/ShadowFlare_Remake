using System;
using UnityEngine;

namespace ShadowFlareRemake.Npc
{
    [Serializable]
    public class DialogTextData
    {
        public string Title;
        public string DialogText;
        [Space(10)]
        public string FirstAnswerTitle;
        public string SecondAnswerTitle;
        public string ThirdAnswerTitle;
    }
}
