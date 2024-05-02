using System;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI {
    public class StatsView : View<StatsModel> {

      
        [Header("References")]
        [SerializeField] private GameObject _statsPanel;

        [Header("Text")]
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _StrengthText;
        [SerializeField] private TMP_Text _AttackText;
        [SerializeField] private TMP_Text _DefenseText;

        protected override void ModelChanged() {

            _statsPanel.SetActive(Model.IsStatsOpen);

            if (!Model.IsStatsOpen ) {
                return;
            }

            _hpText.text = $"{Model.Unit.CurrentHP}/{Model.Stats.MaxHP}";
            _StrengthText.text = Model.Stats.Strength.ToString();
            _AttackText.text = Model.Stats.Attack.ToString();
            _DefenseText.text = Model.Stats.Defense.ToString();
        }
    }
}
