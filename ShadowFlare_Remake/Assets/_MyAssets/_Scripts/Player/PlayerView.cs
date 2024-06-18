using ShadowFlareRemake.Enums;
using System;
using System.Collections;
using UnityEngine;

namespace ShadowFlareRemake.Player
{
    public class PlayerView : View<PlayerModel>
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action OnAttackAnimationEnded;
        public event Action OnDoStepForwardAnimationEvent;

        [Header("Melee Attack Helpers")]
        [SerializeField] private BoxCollider _swordBoxCollider;
        [SerializeField] private GameObject _swordTrailObject;

        [Header("Animation")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private AnimationClip _meleeFirstAttackAnimation;
        [SerializeField] private AnimationClip _meleeSecondAttackAnimation;
        [SerializeField] private AnimationClip _meleeThirdAttackAnimation;

        private float _meleeSingleAttackAnimLength;
        private float _meleeSeoncdAttackAnimLength;
        private float _meleeThirdAttackAnimLength;
        private float _meleeTripleAttackAnimLength;

        private const string _meleeSingleAttackTrigger = "MeleeSingle";
        private const string _meleeTripleAttackTrigger = "MeleeTriple";
        private const string _isMovingBool = "IsMoving";

        #region View Overrides

        protected override void Initialize()
        {
            if(_playerAnimator == null)
                _playerAnimator = GetComponent<Animator>();

            SetAnimationDurations();
        }

        protected override void ModelChanged()
        {
            _playerAnimator.SetBool(_isMovingBool, Model.IsMoving);

            if(Model.IsAttacking)
            {
                HandleAttackAnimations();
            }
        }

        #endregion

        #region MonoBehaviour

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        #endregion

        #region Initializaion

        private void SetAnimationDurations()
        {
            _meleeSingleAttackAnimLength = _meleeFirstAttackAnimation.length;
            _meleeSeoncdAttackAnimLength = _meleeSecondAttackAnimation.length;
            _meleeThirdAttackAnimLength = _meleeThirdAttackAnimation.length;
            _meleeTripleAttackAnimLength = _meleeSingleAttackAnimLength + _meleeSeoncdAttackAnimLength + _meleeThirdAttackAnimLength;
        }

        #endregion

        #region Meat & Potatos

        private void HandleAttackAnimations()
        {
            if(Model.CurrentPlayerAttack == PlayerAttack.None)
                return;

            switch(Model.CurrentPlayerAttack)
            {
                case PlayerAttack.MeleeSingle:
                    DoMeleeSingleAttack();
                    break;

                case PlayerAttack.MeleeTriple:    
                    DoMeleeTripleAttack();
                    break;
            }
        }

        private void SetMeleeAttackActivness(bool isActive)
        {
            _swordBoxCollider.enabled = isActive;
            _swordTrailObject.SetActive(isActive);
        }

        public void DoMeleeSingleAttack()
        {
            _playerAnimator.SetTrigger(_meleeSingleAttackTrigger);
            SetMeleeAttackActivness(true);
            StartCoroutine(WaitForAnimationEnd(_meleeSingleAttackAnimLength));
        }

        public void DoMeleeTripleAttack()
        {
            _playerAnimator.SetTrigger(_meleeTripleAttackTrigger);
            SetMeleeAttackActivness(true);
            StartCoroutine(WaitForAnimationEnd(_meleeTripleAttackAnimLength));
        }

        private IEnumerator WaitForAnimationEnd(float animDuration)
        {
            yield return new WaitForSeconds(animDuration);

            SetMeleeAttackActivness(false);
            OnAttackAnimationEnded?.Invoke();
        }

        public void DoStepForward() 
        { 
            OnDoStepForwardAnimationEvent?.Invoke();
        }

        #endregion
    }
}

