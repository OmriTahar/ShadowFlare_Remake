using System.Linq;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.Npc
{
    public class NpcView : View<NpcModel>
    {
        public string Name { get => name; } //{ get => Model.Name; }
        public float DialogBubbleOffset { get => _dialogBubbleOffset; }
        public int CurrentDialogTextId { get => _currentDialogTextId; }
        public bool IsTalking { get; private set; }

        [Header("Temp")]
        [SerializeField] private string _namePlaceHolder;

        [Header("Dialog Lines")]
        [SerializeField] private string[] _dialogTexts;

        [Header("Dialog Text Data")]
        [SerializeField] private DialogTextData[] _dialogTextsData;

        [Header("Settings")]
        [SerializeField] private float _dialogBubbleOffset = 200;

        private int _currentDialogTextId = 0;

        protected override void ModelChanged() { }

        public DialogTextData GetCurrentDialogTextData(int nextDialogTextId = -1)
        {
            if(_dialogTextsData == null || _dialogTextsData.Length == 0)
                return null;

            DialogTextData currentDialogText = null;

            if(nextDialogTextId > -1)
            {
                currentDialogText = _dialogTextsData.FirstOrDefault(data => data.Id == nextDialogTextId);
                _currentDialogTextId = nextDialogTextId;
            }
            else if(CurrentDialogTextId > _dialogTexts.Length - 1)
            {
                _currentDialogTextId = 0;
            }
            else
            {
                currentDialogText = _dialogTextsData[_currentDialogTextId];
                _currentDialogTextId++;
            }

            return currentDialogText;
        }

        public string GetCurrentDialogTextOld()
        {
            string currentDialogText = null;

            if(CurrentDialogTextId > _dialogTexts.Length - 1)
            {
                _currentDialogTextId = 0;
            }
            else
            {
                currentDialogText = _dialogTexts[_currentDialogTextId];
                _currentDialogTextId++;
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

        public void ResetCurrentDialogTextId()
        {
            _currentDialogTextId = 0;
        }
    }
}
