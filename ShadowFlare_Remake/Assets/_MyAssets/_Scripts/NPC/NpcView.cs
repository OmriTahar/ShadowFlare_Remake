using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.NPC
{
    public class NpcView : View<NpcModel>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _name;

        [Header("Temp")]
        [SerializeField] private string _namePlaceHolder;

        [Header("Speech")]
        [SerializeField] private string[] _speeches;

        private int _currentSpeechIndex = 0;

        private void Start()
        {
            _name.text = _namePlaceHolder;

        }

        protected override void ModelChanged()
        {
            throw new System.NotImplementedException();
        }

        public string GetCurrentSpeech()
        {
            var speechesAmount = _speeches.Length;

            var currentSpeech = _speeches[_currentSpeechIndex];

            var nextSpeechIndex = _currentSpeechIndex + 1;

            if(nextSpeechIndex > _speeches.Length - 1)
            {
                _currentSpeechIndex = 0;
            }
            else
            {
                _currentSpeechIndex = nextSpeechIndex;
            }

            return currentSpeech;
        }
    }
}
