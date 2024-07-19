using ShadowFlare_Remake.VFX;
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

        [Header("References")]
        [SerializeField] private PlayerAnimationsSubView _animaionsSubView;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private VisualEffectsSubView _vfxView;

        private readonly int _movementAnimSpeedParam = Animator.StringToHash("MovementAnimSpeed");
        private readonly int _attackAnimSpeedParam = Animator.StringToHash("AttackAnimSpeed");

        private const string _meleeSingleAttackTrigger = "MeleeSingle";
        private const string _meleeTripleAttackTrigger = "MeleeTriple";
        private const string _isMovingBool = "IsMoving";
        private const int _oneHundred = 100;

        private int _lastSeenHP;

        #region View Overrides

        protected override void Initialize()
        {
            CacheNulls();
            RegisterEvents();
        }

        protected override void ModelReplaced()
        {
            _lastSeenHP = Model.Unit.CurrentHP;
        }

        protected override void ModelChanged()
        {
            SetAnimationsSpeed();
            SetIsMovingAnimation();
            HandleHitEffect();
            HandleHP();

            if(Model.IsAttacking)
            {
                HandleAttackAnimations();
            }
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
            _animaionsSubView.OnFinished_MeleeSingleAttack += InvokeOnAttackAnimationEnded;
            _animaionsSubView.OnFinished_MeleeTripleAttack += InvokeOnAttackAnimationEnded;
        }
        
        private void DeregisterEvents()
        {
            _animaionsSubView.OnDo_StepForwardAnimationEvent -= InvokeDoStepForward;
            _animaionsSubView.OnFinished_MeleeSingleAttack -= InvokeOnAttackAnimationEnded;
            _animaionsSubView.OnFinished_MeleeTripleAttack -= InvokeOnAttackAnimationEnded;
        }

        #endregion

        #region Meat & Potatos

        private void SetAnimationsSpeed()
        {
            _playerAnimator.SetFloat(_movementAnimSpeedParam, Model.GetMovementSpeedForMoveAnimation());
            _playerAnimator.SetFloat(_attackAnimSpeedParam, Model.GetAttackSpeedForAttackAnimations());
        }

        private void SetIsMovingAnimation()
        {
            _playerAnimator.SetBool(_isMovingBool, Model.IsMoving);
        }

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
        }

        private void DoMeleeTripleAttack()
        {
            _playerAnimator.SetTrigger(_meleeTripleAttackTrigger);
        }

        private void InvokeDoStepForward() 
        {
            OnDoStepForwardAnimationEvent?.Invoke();
        }

        private void InvokeOnAttackAnimationEnded()
        {
            OnAttackAnimationEnded?.Invoke();
        }

        private void HandleHP()
        {
            // Check if not already dead

            var realHP = Model.Unit.CurrentHP;
            _lastSeenHP = realHP < 0 ? 0 : realHP;

            //if(_lastSeenHP == 0)
            //{
            //    OnDeath?.Invoke();
            //}
        }

        private void HandleHitEffect()
        {
            if(Model.Unit.CurrentHP >= _lastSeenHP)
                return;

            if(Model.IsLastHitWasCritialHit)
            {
                _vfxView.SetIsPlayingEffect(VfxType.HitBlood, true);
                return;
            }

            _vfxView.SetIsPlayingEffect(VfxType.Hit, true);
        }

        #endregion
    }
}

