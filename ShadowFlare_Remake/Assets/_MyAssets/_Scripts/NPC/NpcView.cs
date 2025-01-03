using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.Npc
{
    public class NpcView : View<NpcModel>
    {
        public string Name { get => Model.Name; }
        public float DialogBubbleOffset { get => _dialogBubbleOffset; }
        public bool IsTalking { get; private set; }

        [Header("References")]
        [SerializeField] private TMP_Text _name;

        [Header("Temp")]
        [SerializeField] private string _namePlaceHolder;

        [Header("Dialog Lines")]
        [SerializeField] private string[] _dialogTexts;

        [Header("Dialog Text Data")]
        [SerializeField] private DialogTextData[] _dialogTextsData;

        [Header("Settings")]
        [SerializeField] private float _dialogBubbleOffset = 200;

        public int CurrentDialogTextId = 0; // Fix this public shit -> Use the fucking model

        private void Start()
        {
            _name.text = _namePlaceHolder;
        }

        protected override void ModelChanged() { }

        public DialogTextData GetCurrentDialogTextData(int nextDialogTextId = -1)
        {
            if(_dialogTextsData == null || _dialogTextsData.Length == 0)
                return null;

            DialogTextData currentDialogText = null;

            if(nextDialogTextId > -1)
            {
                currentDialogText = _dialogTextsData.FirstOrDefault(data => data.Id == nextDialogTextId);
                CurrentDialogTextId = nextDialogTextId;
            }
            else if(CurrentDialogTextId > _dialogTexts.Length - 1)
            {
                CurrentDialogTextId = 0;
            }
            else
            {
                currentDialogText = _dialogTextsData[CurrentDialogTextId];
                CurrentDialogTextId++;
            }

            return currentDialogText;
        }

        public string GetCurrentDialogTextOld()
        {
            string currentDialogText = null;

            if(CurrentDialogTextId > _dialogTexts.Length - 1)
            {
                CurrentDialogTextId = 0;
            }
            else
            {
                currentDialogText = _dialogTexts[CurrentDialogTextId];
                CurrentDialogTextId++;
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
