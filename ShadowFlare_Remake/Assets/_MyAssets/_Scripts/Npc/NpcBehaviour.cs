using System.Linq;
using UnityEngine;

namespace ShadowFlareRemake.Npc
{
    public class NpcBehaviour : MonoBehaviour
    {
        public int CurrentDialogTextId { get; private set; } = 0;
        public bool IsTalking { get; private set; }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public NpcType NpcType { get; private set; }  
        [field: SerializeField] public float DialogBubbleOffset { get; private set; } = 200;

        [Header("Dialog")]
        [SerializeField] private DialogTextData[] _dialogTextDataArray;

        private readonly int _castAnimHash = Animator.StringToHash("Cast");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public DialogTextData GetNextDialogTextData(int nextDialogTextId)
        {
            if(_dialogTextDataArray == null || _dialogTextDataArray.Length == 0)
                return null;

            if(nextDialogTextId > -1)
            {
                return GetDialogTextDataById(nextDialogTextId);
            }

            return GetNextDialogTextData_Internal();
        }

        private DialogTextData GetDialogTextDataById(int nextDialogTextId)
        {
            CurrentDialogTextId = nextDialogTextId;
            return _dialogTextDataArray.FirstOrDefault(data => data.Id == nextDialogTextId);
        }

        private DialogTextData GetNextDialogTextData_Internal()
        {
            if(CurrentDialogTextId >= _dialogTextDataArray.Length)
            {
                CurrentDialogTextId = 0;
            }

            DialogTextData currentDialogText = _dialogTextDataArray[CurrentDialogTextId];
            CurrentDialogTextId++;
            return currentDialogText;
        }

        public bool IsNextDialogIsQuestion()
        {
            if(CurrentDialogTextId > _dialogTextDataArray.Length)
            {
                return false;
            }

            return _dialogTextDataArray[CurrentDialogTextId].IsQuestionText;
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
            CurrentDialogTextId = 0;
        }

        public void CastHealAnimation()
        {
            _animator.SetTrigger(_castAnimHash);
        }
    }
}
