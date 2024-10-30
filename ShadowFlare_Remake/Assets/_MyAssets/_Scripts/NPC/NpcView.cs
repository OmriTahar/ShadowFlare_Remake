using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.NPC
{
    public class NpcView : View<NpcModel>
    {
        public float SpeechBubbleOffset { get => _speechBubbleOffset; }
        public bool IsTalking {get; private set;}

        [Header("References")]
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _speechText;
        [SerializeField] private GameObject _speechHolder;

        [Header("Temp")]
        [SerializeField] private string _namePlaceHolder;

        [Header("Speech")]
        [SerializeField] private string[] _speeches;

        [Header("Settings")]
        [SerializeField] private float _speechBubbleOffset = 200;

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
            string currentSpeech = null;

            if(_currentSpeechIndex > _speeches.Length - 1)
            {
                _currentSpeechIndex = 0;
            }
            else
            {
                currentSpeech = _speeches[_currentSpeechIndex];
                _currentSpeechIndex++;
            }

            return currentSpeech;
        }

        public void SetIsSpeechHolderEnabled(bool isEnabled)
        {
            _speechHolder.SetActive(isEnabled);
        }

        public void SetIsTalking(bool isTalking)
        {
            IsTalking = isTalking;
        }

        public NpcModel GetNpcModel()
        {
            return Model;
        }

        public void LookAtPlayer(Transform playerTransform)
        {
            transform.LookAt(playerTransform);
        }
    }
}
