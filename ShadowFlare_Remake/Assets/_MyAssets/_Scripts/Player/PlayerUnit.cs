using ShadowFlareRemake.UI;
using UnityEngine;

namespace ShadowFlareRemake.Player {
    public class PlayerUnit : Unit {

        [Header("References")]
        [SerializeField] private UIController _uiController;

        [Header("Debug")]
        [SerializeField] private IUnitStats _playerUnitStatsCopy;
        [SerializeField] private bool _restartUnitStats = true;

        public PlayerUnit(PlayerStats stats) : base(stats) {
        }

        //protected override void Awake() {

        //    base.Awake();
        //    HandleRestartUnitStats();
        //}

        protected override void FullHeal(bool notifyUiController) {

            if(notifyUiController) {
                _uiController.UpdatePlayerHPAndMP();
            }
        }

        public override void TakeDamage(int damage) {

            base.TakeDamage(damage);
            _uiController.UpdatePlayerHPAndMP();
        }

        public override void HealHP(int hpAmount) {

            base.HealHP(hpAmount);
            _uiController.UpdatePlayerHPAndMP();
        }

        public override void HealMP(int mpAmount) {

            base.HealMP(mpAmount);
            _uiController.UpdatePlayerHPAndMP();
        }

        //private void HandleRestartUnitStats() {

        //    var b = _playerUnitStatsCopy;

        //    if(_restartUnitStats) {

        //        Stats.Vcocation = b.Vcocation;
        //        Stats.CurrentExp = b.CurrentExp;
        //        Stats.ExpToLevelUp = b.ExpToLevelUp;

        //        Stats.Name = b.Name;
        //        Stats.Level = b.Level;
        //        Stats.MaxHP = b.MaxHP;
        //        Stats.Strength = b.Strength;
        //        Stats.Attack = b.Attack;
        //        Stats.Defence = b.Defence;
        //        Stats.HitRate = b.HitRate;
        //        Stats.EvasionRate = b.EvasionRate;
        //        Stats.WalkingSpeed = b.WalkingSpeed;
        //        Stats.AttackSpeed = b.AttackSpeed;

        //        Stats.MaxMP = b.MaxMP;
        //        Stats.MagicalAttack = b.MagicalAttack;
        //        Stats.MagicalDefence = b.MagicalDefence;
        //        Stats.MagicalHitRate = b.MagicalHitRate;
        //        Stats.MagicalEvasionRate = b.MagicalEvasionRate;
        //        Stats.MagicalAttackSpeed = b.MagicalAttackSpeed;
        //    }

        //}
    }
}
