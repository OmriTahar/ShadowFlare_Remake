using System;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI {
    public class LevelUpView : View<LevelUpModel> {

        public event Action OnPanelClicked;

        [Header("References")]
        [SerializeField] private GameObject _levelUpPanel;

        [Header("Texts")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _mpText;
        [SerializeField] private TMP_Text _strengthText;
        [SerializeField] private TMP_Text _attackText;
        [SerializeField] private TMP_Text _defenseText;
        [SerializeField] private TMP_Text _magicalAttackText;
        [SerializeField] private TMP_Text _magicalDefenseText;
        [SerializeField] private TMP_Text _addedSpellText;
        [SerializeField] private TMP_Text _occupationText;


        protected override void ModelChanged() {

            _levelUpPanel.SetActive(Model.IsPanelOpen);

            if(!Model.IsPanelOpen) {
                return;
            }

            _levelText.text = Model.Level.ToString();
            _hpText.text = Model.HP.ToString();
            _mpText.text = Model.MP.ToString();
            _strengthText.text = Model.Strength.ToString();

            _attackText.text = Model.Attack.ToString();
            _defenseText.text = Model.Defense.ToString();
            _magicalAttackText.text = Model.MagicalAttack.ToString();
            _magicalDefenseText.text = Model.MagicalDefence.ToString();
        }

        public void PanelClicked() {

            OnPanelClicked?.Invoke();
        }
    }
}
