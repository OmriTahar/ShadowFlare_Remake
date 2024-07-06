using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI.Stats {
    public class StatsView : UIView<StatsModel> {

        [Header("References")]
        [SerializeField] private GameObject _statsPanel;

        [Header("Base Texts")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _vocationText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _expText;

        [Header("Physical Texts")]
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _strengthText;
        [SerializeField] private TMP_Text _attackText;
        [SerializeField] private TMP_Text _defenseText;
        [SerializeField] private TMP_Text _hitRateText;
        [SerializeField] private TMP_Text _evasionRateText;
        [SerializeField] private TMP_Text _movementSpeedText;
        [SerializeField] private TMP_Text _attackSpeedText;

        [Header("Magical Texts")]
        [SerializeField] private TMP_Text _mpText;
        [SerializeField] private TMP_Text _magicalAttackText;
        [SerializeField] private TMP_Text _magicalDefenseText;
        [SerializeField] private TMP_Text _magicalAttackSpeedText;
        [SerializeField] private TMP_Text _magicalHitRateText;
        [SerializeField] private TMP_Text _magicalEvasionRateText;

        protected override void ModelChanged() {

            _statsPanel.SetActive(Model.IsPanelOpen);

            if(!Model.IsPanelOpen) {
                return;
            }

            SetBaseStats();
            SetPhysicalStats();
            SetMagicalStats();
        }

        private void SetBaseStats()
        {
            _nameText.text = Model.Stats.Name;
            _vocationText.text = Model.Stats.Vocation.ToString();
            _levelText.text = Model.Stats.Level.ToString();
            _expText.text = $"{Model.Stats.CurrentExp} / {Model.Stats.ExpToLevelUp}";
        }

        private void SetPhysicalStats()
        {
            _hpText.text = $"{Model.Unit.CurrentHP} / {Model.Stats.MaxHP}";
            _strengthText.text = Model.Stats.Strength.ToString();
            _attackText.text = Model.Stats.Attack.ToString();
            _defenseText.text = Model.Stats.Defense.ToString();
            _hitRateText.text = Model.Stats.HitRate.ToString();
            _evasionRateText.text = Model.Stats.EvasionRate.ToString();
            _movementSpeedText.text = Model.Stats.MovementSpeed.ToString();
            _attackSpeedText.text = Model.Stats.AttackSpeed.ToString();
        }

        private void SetMagicalStats()
        {
            _mpText.text = $"{Model.Unit.CurrentMP} / {Model.Stats.MaxMP}";
            _magicalAttackText.text = Model.Stats.MagicalAttack.ToString();
            _magicalDefenseText.text = Model.Stats.MagicalDefense.ToString();
            _magicalAttackSpeedText.text = Model.Stats.MagicalAttackSpeed.ToString();
            _magicalHitRateText.text = Model.Stats.MagicalHitRate.ToString();
            _magicalEvasionRateText.text = Model.Stats.MagicalEvasionRate.ToString();
        }
    }
}
