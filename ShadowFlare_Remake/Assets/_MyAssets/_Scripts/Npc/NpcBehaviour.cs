using System.Linq;
using UnityEngine;

namespace ShadowFlareRemake.Npc
{
    public class NpcBehaviour : MonoBehaviour
    {
        public string Name { get => _name; } 
        public float DialogBubbleOffset { get => _dialogBubbleOffset; }
        public int CurrentDialogTextId { get => _currentDialogTextId; }
        public bool IsTalking { get; private set; }

        [Header("Initialization")]
        [SerializeField] private string _name;

        [Header("Dialog")]
        [SerializeField] private DialogTextData[] _dialogTextDataArray;

        [Header("Settings")]
        [SerializeField] private float _dialogBubbleOffset = 200;

        private int _currentDialogTextId = 0;

        public DialogTextData GetCurrentDialogTextData(int nextDialogTextId)
        {
            if(_dialogTextDataArray == null || _dialogTextDataArray.Length == 0)
                return null;

            if(nextDialogTextId > -1)
            {
                return GetDialogTextDataById(nextDialogTextId);
            }

            return GetNextDialogTextData();
        }

        private DialogTextData GetDialogTextDataById(int nextDialogTextId)
        {
            _currentDialogTextId = nextDialogTextId;
            return _dialogTextDataArray.FirstOrDefault(data => data.Id == nextDialogTextId);
        }

        private DialogTextData GetNextDialogTextData()
        {
            if(_currentDialogTextId >= _dialogTextDataArray.Length)
            {
                _currentDialogTextId = 0;
            }

            DialogTextData currentDialogText = _dialogTextDataArray[_currentDialogTextId];
            _currentDialogTextId++;
            return currentDialogText;
        }

        public bool IsNextDialogIsQuestion()
        {
            if(_currentDialogTextId > _dialogTextDataArray.Length)
            {
                return false;
            }

            return _dialogTextDataArray[_currentDialogTextId].IsQuestionText;
        }

        public void SetIsTalking(bool isTalking)
        {
            IsTalking = isTalking;
        }

        public void LookAtTransform(Transform transformToLookAt)
        {
            transform.LookAt(transformToLookAt);
        }

        public void ResetCurrentDialogTextId()
        {
            _currentDialogTextId = 0;
        }
    }
}
