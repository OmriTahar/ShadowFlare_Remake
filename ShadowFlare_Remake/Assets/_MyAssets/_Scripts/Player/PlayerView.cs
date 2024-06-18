using ShadowFlareRemake.Enums;
using System;
using UnityEngine;

namespace ShadowFlareRemake.Player
{
    public class PlayerView : View<PlayerModel>
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action OnDoStepForwardAnimationEvent;
        public event Action OnAttackAnimationEnded;

        [Header("Sub Views")]
        [SerializeField] private PlayerAnimationsSubView _animaionsSubView;

        [Header("Melee Attack Helpers")]
        [SerializeField] private BoxCollider _swordBoxCollider;
        [SerializeField] private GameObject _swordTrailObject;

        [Header("Animation")]
        [SerializeField] private Animator _playerAnimator;

        private const string _meleeSingleAttackTrigger = "MeleeSingle";
        private const string _meleeTripleAttackTrigger = "MeleeTriple";
        private const string _isMovingBool = "IsMoving";

        #region View Overrides

        protected override void Initialize()
        {
            CacheNulls();
            RegisterEvents();
        }

        protected override void ModelChanged()
        {
            _playerAnimator.SetBool(_isMovingBool, Model.IsMoving);

            if(Model.IsAttacking)
                HandleAttackAnimations();
        }

        protected override void Clean()
        {
            DeregisterEvents();
        }

        #endregion

        #region MonoBehaviour

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        #endregion

        #region Initializaion

        private void CacheNulls()
        {
            if(_playerAnimator == null)
                _playerAnimator = GetComponent<Animator>();
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _animaionsSubView.OnDo_StepForwardAnimationEvent += InvokeDoStepForward;
            _animaionsSubView.OnFinished_MeleeSingleAttack += FinishedMeleeAttack;
            _animaionsSubView.OnFinished_MeleeTripleAttack += FinishedMeleeAttack;
        }
        
        private void DeregisterEvents()
        {
            _animaionsSubView.OnDo_StepForwardAnimationEvent -= InvokeDoStepForward;
            _animaionsSubView.OnFinished_MeleeSingleAttack -= FinishedMeleeAttack;
            _animaionsSubView.OnFinished_MeleeTripleAttack -= FinishedMeleeAttack;
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

        private void DoMeleeSingleAttack()
        {
            _playerAnimator.SetTrigger(_meleeSingleAttackTrigger);
            SetMeleeAttackActivness(true);
        }

        private void DoMeleeTripleAttack()
        {
            _playerAnimator.SetTrigger(_meleeTripleAttackTrigger);
            SetMeleeAttackActivness(true);
        }

        private void SetMeleeAttackActivness(bool isActive)
        {
            _swordBoxCollider.enabled = isActive;
            _swordTrailObject.SetActive(isActive);
        }

        private void InvokeDoStepForward() 
        {
            OnDoStepForwardAnimationEvent?.Invoke();
        }

        private void InvokeFinishedAttackAnimation()
        {
            OnAttackAnimationEnded?.Invoke();
        }

        private void FinishedMeleeAttack()
        {
            SetMeleeAttackActivness(false);
            InvokeFinishedAttackAnimation();
        }

        #endregion
    }
}

