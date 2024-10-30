using System;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogView : UIView<DialogModel>
    {
        public event Action<int> OnAnswerClicked;

        [Header("References")]
        [SerializeField] private Transform _dialogBubbleHolder;
        [SerializeField] private TMP_Text _dialogText;

        [Header("Answers Refs")]
        [SerializeField] private GameObject _firstAnswerGO;
        [SerializeField] private TMP_Text _firstAnswerText;
        [Space(10)]
        [SerializeField] private GameObject _secondAnswerGO;
        [SerializeField] private TMP_Text _secondAnswerText;
        [Space(10)]
        [SerializeField] private GameObject _thirdAnswerGO;
        [SerializeField] private TMP_Text _thirdAnswerText;

        protected override void ModelChanged()
        {
            _dialogBubbleHolder.gameObject.SetActive(Model.IsBubbleActive);
            _dialogBubbleHolder.position = Model.DialogBubblePosition;
            SetDialogText();
            SetDialogAnswers();
        }

        private void SetDialogText()
        {
            if(Model.CurrentDialogTextData == null)
                return;

            _dialogText.text = Model.CurrentDialogTextData.DialogText;
        }

        private void SetDialogAnswers()
        {
            if(Model.CurrentDialogTextData == null)
                return;

            var fisrtAnswer = Model.CurrentDialogTextData.FirstAnswerTitle;
            var secondAnswer = Model.CurrentDialogTextData.SecondAnswerTitle;
            var thirdAnswer = Model.CurrentDialogTextData.ThirdAnswerTitle;

            if(!string.IsNullOrEmpty(fisrtAnswer))
            {
                _firstAnswerGO.SetActive(true);
                _firstAnswerText.text = fisrtAnswer;
            }
            else
            {
                _firstAnswerGO.SetActive(false);
            }

            if(!string.IsNullOrEmpty(secondAnswer))
            {
                _secondAnswerGO.SetActive(true);
                _secondAnswerText.text = secondAnswer;
            }
            else
            {
                _secondAnswerGO.SetActive(false);
            }

            if(!string.IsNullOrEmpty(thirdAnswer))
            {
               _thirdAnswerGO.SetActive(true);
                _thirdAnswerText.text = thirdAnswer;
            }
            else
            {
                _thirdAnswerGO.SetActive(false);
            }
        }

        #region Unity Buttons

        public void FirstAnswerClicked()
        {
            OnAnswerClicked?.Invoke(0);
        }

        public void SecondAnswerClicked()
        {
            OnAnswerClicked?.Invoke(1);
        }

        public void ThirdAnswerClicked()
        {
            OnAnswerClicked?.Invoke(2);
        }

        #endregion
    }
}
