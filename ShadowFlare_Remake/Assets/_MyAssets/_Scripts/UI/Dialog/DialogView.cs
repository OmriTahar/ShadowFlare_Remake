using System;
using System.Linq;
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

            var text = ReplaceBackslashWithNewline(Model.CurrentDialogTextData.DialogText);
            _dialogText.text = text;
        }

        public string ReplaceBackslashWithNewline(string input)
        {
            char[] chars = input.ToCharArray();

            for(int i = 0; i < chars.Length; i++)
            {
                if(chars[i] == '\\')
                {
                    chars[i] = '\n';
                }
            }

            return new string(chars);
        }

        private void SetDialogAnswers()
        {
            var dialogData = Model.CurrentDialogTextData;

            if(!IsValidQuestion())
            {
                TurnOffDialogAnswers();
                return;
            }

            var fisrtAnswer = dialogData.Answers[0].Title;
            var secondAnswer = dialogData.Answers[1].Title;
            var thirdAnswer = dialogData.Answers[2].Title;

            SetAnswerTextAndActivness(_firstAnswerGO, _firstAnswerText, fisrtAnswer);
            SetAnswerTextAndActivness(_secondAnswerGO, _secondAnswerText, secondAnswer);
            SetAnswerTextAndActivness(_thirdAnswerGO, _thirdAnswerText, thirdAnswer);
        }
        private bool IsValidQuestion()
        {
            var dialogData = Model.CurrentDialogTextData;

            if(dialogData == null || !dialogData.IsQuestionText || dialogData.Answers == null || dialogData.Answers.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void SetAnswerTextAndActivness(GameObject textGameObject, TMP_Text tmpText, string Answer)
        {
            if(string.IsNullOrEmpty(Answer))
            {
                textGameObject.SetActive(false);
                return;
            }

            textGameObject.SetActive(true);
            tmpText.text = Answer;
        }

        private void TurnOffDialogAnswers()
        {
            _firstAnswerGO.SetActive(false);
            _secondAnswerGO.SetActive(false);
            _thirdAnswerGO.SetActive(false);
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
