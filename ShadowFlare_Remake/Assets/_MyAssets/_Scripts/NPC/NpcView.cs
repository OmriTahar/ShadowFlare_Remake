using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.NPC
{
    public class NpcView : View<NpcModel>
    {
        public float DialogBubbleOffset { get => _dialogBubbleOffset; }
        public bool IsTalking { get; private set; }

        [Header("References")]
        [SerializeField] private TMP_Text _name;

        [Header("Temp")]
        [SerializeField] private string _namePlaceHolder;

        [Header("Dialog Lines")]
        [SerializeField] private string[] _dialogTexts;

        [Header("Settings")]
        [SerializeField] private float _dialogBubbleOffset = 200;

        private int _dialogTextsIndex = 0;

        private void Start()
        {
            _name.text = _namePlaceHolder;
        }

        protected override void ModelChanged() { }

        public string GetCurrentDialogText()
        {
            string currentDialogText = null;

            if(_dialogTextsIndex > _dialogTexts.Length - 1)
            {
                _dialogTextsIndex = 0;
            }
            else
            {
                currentDialogText = _dialogTexts[_dialogTextsIndex];
                _dialogTextsIndex++;
            }

            return currentDialogText;
        }

        public void SetIsTalking(bool isTalking)
        {
            IsTalking = isTalking;
        }

        public void LookAtPlayer(Transform playerTransform)
        {
            transform.LookAt(playerTransform);
        }
    }
}
