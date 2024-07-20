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

        [Header("Settings")]
        [SerializeField] private Color _statDefaultColor;
        [SerializeField] private Color _statBuffedColor;
        [SerializeField] private Color _statNerfedColor;

        protected override void ModelChanged() {

            _statsPanel.SetActive(Model.IsPanelOpen);

            if(!Model.IsPanelOpen) {
                return;
            }

            SetExpAndLevelStats();

            if(Model.IsFullStatsUpdate)
            {
                var isNerfed = Model.Stats.EquippedWeight > Model.Stats.Strength;

                SetBaseStats();
                SetPhysicalStats(isNerfed);
                SetMagicalStats(isNerfed);
            }
        }

        private void SetExpAndLevelStats()
        {
            _levelText.text = Model.Stats.Level.ToString();
            _expText.text = $"{Model.Stats.CurrentExp} / {Model.Stats.ExpToLevelUp}";
        }

        private void SetBaseStats()
        {
            _nameText.text = Model.Stats.Name;
            _vocationText.text = Model.Stats.Vocation.ToString();
        }

        private void SetPhysicalStats(bool isNerfed)
        {
            _hpText.text = $"{Model.Unit.CurrentHP} / {Model.Stats.MaxHP}";
            _strengthText.text = Model.Stats.Strength.ToString();
            _attackText.text = Model.Stats.Attack.ToString();
            _defenseText.text = Model.Stats.Defense.ToString();
            _hitRateText.text = Model.Stats.HitRate.ToString();
            _evasionRateText.text = Model.Stats.EvasionRate.ToString();
            _movementSpeedText.text = Model.Stats.MovementSpeed.ToString();
            _attackSpeedText.text = Model.Stats.AttackSpeed.ToString();

            SetBuffedTextColor(_strengthText, Model.EquippedGearAddedStats.Strength);
            SetBuffedTextColor(_attackText, Model.EquippedGearAddedStats.Attack);
            SetBuffedTextColor(_defenseText, Model.EquippedGearAddedStats.Defense);
            SetBuffedTextColor(_hitRateText, Model.EquippedGearAddedStats.HitRate);
            SetBuffedTextColor(_evasionRateText, Model.EquippedGearAddedStats.EvasionRate);
            SetBuffedTextColor(_movementSpeedText, Model.EquippedGearAddedStats.MovementSpeed, isNerfed);
            SetBuffedTextColor(_attackSpeedText, Model.EquippedGearAddedStats.AttackSpeed, isNerfed);
        }

        private void SetMagicalStats(bool isNerfed)
        {
            _mpText.text = $"{Model.Unit.CurrentMP} / {Model.Stats.MaxMP}";
            _magicalAttackText.text = Model.Stats.MagicalAttack.ToString();
            _magicalDefenseText.text = Model.Stats.MagicalDefense.ToString();
            _magicalAttackSpeedText.text = Model.Stats.MagicalAttackSpeed.ToString();
            _magicalHitRateText.text = Model.Stats.MagicalHitRate.ToString();
            _magicalEvasionRateText.text = Model.Stats.MagicalEvasionRate.ToString();

            SetBuffedTextColor(_magicalAttackText, Model.EquippedGearAddedStats.MagicalAttack);
            SetBuffedTextColor(_magicalDefenseText, Model.EquippedGearAddedStats.MagicalDefense);
            SetBuffedTextColor(_magicalAttackSpeedText, Model.EquippedGearAddedStats.MagicalAttackSpeed, isNerfed);
            SetBuffedTextColor(_magicalHitRateText, Model.EquippedGearAddedStats.MagicalHitRate);
            SetBuffedTextColor(_magicalEvasionRateText, Model.EquippedGearAddedStats.MagicalEvasionRate);
        }

        private void SetBuffedTextColor(TMP_Text text, int equippedGearAddedStat, bool isNerfed = false)
        {
            if(isNerfed)
            {
                text.color = _statNerfedColor;
                return;
            }

            text.color = equippedGearAddedStat > 0 ? _statBuffedColor : _statDefaultColor;
        }
    }
}
