using System;
using TMPro;
using UnityEngine;

namespace ShadowFlareRemake.UI.LevelUp
{
    public class LevelUpView : UIView<LevelUpModel>
    {
        public event Action OnPanelClicked;

        [Header("References")]
        [SerializeField] private GameObject _levelUpPanel;
        [SerializeField] private Animator _animator;

        [Header("Texts")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _mpText;
        [SerializeField] private TMP_Text _strengthText;
        [SerializeField] private TMP_Text _attackText;
        [SerializeField] private TMP_Text _defenseText;
        [SerializeField] private TMP_Text _magicalAttackText;
        [SerializeField] private TMP_Text _magicalDefenseText;

        // Todo:
        //[SerializeField] private TMP_Text _addedSpellText;
        //[SerializeField] private TMP_Text _occupationText;

        private readonly int _isActiveAnimHash = Animator.StringToHash("IsActive");
        private readonly int _cornerAnimHash = Animator.StringToHash("Corner");


        protected override void ModelChanged()
        {
            SetTexts();
            HandlePanelState();
        }

        private void SetTexts()
        {
            _levelText.text = Model.Level.ToString();
            _hpText.text = Model.HP.ToString();
            _mpText.text = Model.MP.ToString();
            _strengthText.text = Model.Strength.ToString();

            _attackText.text = Model.Attack.ToString();
            _defenseText.text = Model.Defense.ToString();
            _magicalAttackText.text = Model.MagicalAttack.ToString();
            _magicalDefenseText.text = Model.MagicalDefence.ToString();
        }

        private void HandlePanelState()
        {
            switch(Model.State)
            {
                case LevelUpModel.LevelUpPanelState.Hidden:
                    _animator.SetBool(_isActiveAnimHash, false);
                    break;

                case LevelUpModel.LevelUpPanelState.Shown:
                    _animator.SetBool(_isActiveAnimHash, true);
                    break;

                case LevelUpModel.LevelUpPanelState.MovingToCorner:
                    _animator.SetTrigger(_cornerAnimHash);
                    break;

                case LevelUpModel.LevelUpPanelState.FadingOut:
                    _animator.SetBool(_isActiveAnimHash, false);
                    break;

                default:
                    break;
            }
        }

        public void PanelClicked()
        {
            OnPanelClicked?.Invoke();
        }
    }
}
