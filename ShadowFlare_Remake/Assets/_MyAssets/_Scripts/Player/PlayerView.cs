using System;
using System.Collections;
using UnityEngine;

namespace ShadowFlareRemake.Player {
    public class PlayerView : View<PlayerModel> {

        public event Action<Collider> OnTriggerEnterEvent;

        public event Action OnAttackAnimationEnded;
        public event Action OnDoStepForwardAnimationEvent;

        [Header("Animation")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private AnimationClip _singleAttackAnimation;
        [SerializeField] private AnimationClip _threeStrikesAnimation;

        private float _singleAttackAnimLength;
        private float _threeStrikesAnimLength;

        protected override void Initialize() {

            base.Initialize();

            if(_playerAnimator == null) {
                _playerAnimator = GetComponent<Animator>();
            }

            _singleAttackAnimLength = _singleAttackAnimation.length;
            _threeStrikesAnimLength = _threeStrikesAnimation.length;
        }

        private void OnTriggerEnter(Collider other) {
            OnTriggerEnterEvent?.Invoke(other);
        }

        protected override void ModelChanged() {

            if(Model == null) {
                print("Model is null muchacho");
                return;
            }

            if(Model.IsAttacking) {
                HandleAttackAnimations();
            }
        }

        private void HandleHP() {

            //_healthSlider = Model.Unit.CurrentHP;

            //_currentHP = hp < 0 ? 0 : hp;
            //_healthSlider.value = _currentHP;

            //if(_currentHP == 0) {
            //    DoDeath();
            //}
        }

        private void HandleAttackAnimations() {

            switch(Model.CurrentAttackType) {

                case PlayerModel.AttackType.Single:
                    DoSingleAttack();
                    break;

                case PlayerModel.AttackType.ThreeStrikes:
                    DoThreeStrikesAttack();
                    break;
            }
        }

        public void DoSingleAttack() {
            _playerAnimator.SetTrigger("SingleAttack");
            StartCoroutine(WaitForAnimationEnd(_singleAttackAnimLength));
        }

        public void DoThreeStrikesAttack() {
            _playerAnimator.SetTrigger("ThreeStrikes");
            StartCoroutine(WaitForAnimationEnd(_threeStrikesAnimLength));
        }

        private IEnumerator WaitForAnimationEnd(float animDuration) {
            yield return new WaitForSeconds(animDuration);
            OnAttackAnimationEnded?.Invoke();
        }


        // Gets invoked from "ThreeStrikes" animation as an animation event
        public void DoStepForward() {
            OnDoStepForwardAnimationEvent?.Invoke();
        }
    }
}

