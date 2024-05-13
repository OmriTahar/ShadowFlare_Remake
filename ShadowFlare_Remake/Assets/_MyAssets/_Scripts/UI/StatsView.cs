using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI {
    public class StatsView : UIView<StatsModel> {

        [Header("References")]
        [SerializeField] private GameObject _statsPanel;

        [Header("Text")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _currentExpText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _StrengthText;
        [SerializeField] private TMP_Text _AttackText;
        [SerializeField] private TMP_Text _DefenseText;
        [SerializeField] private TMP_Text _mpText;
        [SerializeField] private TMP_Text _magicalAttackText;
        [SerializeField] private TMP_Text _magicalDefenseText;

        protected override void ModelChanged() {

            _statsPanel.SetActive(Model.IsPanelOpen);

            if(!Model.IsPanelOpen) {
                return;
            }

            _levelText.text = Model.Stats.Level.ToString();
            _currentExpText.text = Model.Stats.CurrentExp.ToString();

            _hpText.text = $"{Model.Unit.CurrentHP} / {Model.Stats.MaxHP}";
            _StrengthText.text = Model.Stats.Strength.ToString();
            _AttackText.text = Model.Stats.Attack.ToString();
            _DefenseText.text = Model.Stats.Defense.ToString();

            _mpText.text = Model.Stats.MaxMP.ToString();
            _magicalAttackText.text = Model.Stats.MagicalAttack.ToString();
            _magicalDefenseText.text = Model.Stats.MagicalDefence.ToString();
        }
    }
}
