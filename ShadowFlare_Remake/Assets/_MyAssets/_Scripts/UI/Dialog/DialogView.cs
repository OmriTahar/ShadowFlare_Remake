using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI.Dialog
{
    public class DialogView : UIView<DialogModel>
    {
        [Header("References")]
        [SerializeField] private Transform _speechBubbleHolder;
        [SerializeField] private TMP_Text _dialogText;

        [Header("Answers Refs")]
        [SerializeField] private GameObject _firstAnswerGO;
        [SerializeField] private TMP_Text _firstAnswerText;
        [Space(5)]
        [SerializeField] private GameObject _secondAnswerGO;
        [SerializeField] private TMP_Text _secondAnswerText;
        [Space(5)]
        [SerializeField] private GameObject _thirdAnswerGO;
        [SerializeField] private TMP_Text _thirdAnswerText;

        protected override void ModelChanged()
        {
            _speechBubbleHolder.gameObject.SetActive(Model.IsBubbleActive);
            _speechBubbleHolder.position = Model.SpeechBubblePosition;
            _dialogText.text = Model.CurrentText;
        }
    }
}
