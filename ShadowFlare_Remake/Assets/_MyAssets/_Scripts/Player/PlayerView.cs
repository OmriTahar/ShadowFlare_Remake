using ShadowFlareRemake.Skills;
using ShadowFlareRemake.VFX;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowFlareRemake.Player
{
    public class PlayerView : View<BasePlayerModel>
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action OnDoStepForwardAnimationEvent;
        public event Action OnAttackAnimationEnded;

        [Header("References")]
        [SerializeField] private PlayerAnimationsSubView _animaionsSubView;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private VisualEffectsSubView _vfxView;

        [Header("Settings")]
        [SerializeField] private float _meteorSkillWait = 0.75f;

        private readonly int _movementAnimSpeedParam = Animator.StringToHash("MovementAnimSpeed");
        private readonly int _attackAnimSpeedParam = Animator.StringToHash("AttackAnimSpeed");
        private readonly int _magicalAttackAnimSpeedParam = Animator.StringToHash("MagicalAttackAnimSpeed");

        private const string _isMovingBool = "IsMoving";
        private const string _isTalkingBool = "IsTalking";
        private const string _meleeSingleAttackTrigger = "MeleeSingle";
        private const string _meleeTripleAttackTrigger = "MeleeTriple";
        private const string _castSpellMeteorTrigger = "CastSpell_Meteor";

        private const int _oneHundred = 100;
        private const int _oneSecondInMilliSeconds = 1000;

        private float _magicalAttackSpeed;
        private int _meteorSkillWaitInMilliSeconds;
        private int _lastSeenHP;

        #region View Overrides

        protected override void Initialize()
        {
            CacheNulls();
            RegisterEvents();
        }

        protected override void ModelReplaced()
        {
            InitLastSeenHP();
            SetDelaysInMilliSeconds();
        }

        protected override void ModelChanged()
        {
            SetAnimationsSpeed();
            SetIsMovingAnimation();
            SetIsTalkingAnimation();
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

        private void InitLastSeenHP()
        {
            _lastSeenHP = Model.Unit.CurrentHP;
        }

        private void SetDelaysInMilliSeconds()
        {
            _meteorSkillWaitInMilliSeconds = (int)(_meteorSkillWait * _oneSecondInMilliSeconds);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _animaionsSubView.OnDo_StepForwardAnimationEvent += InvokeDoStepForward;
            _animaionsSubView.OnFinished_MeleeSingleAttack += InvokeOnAttackAnimationEnded;
            _animaionsSubView.OnFinished_MeleeTripleAttack += InvokeOnAttackAnimationEnded;
            _animaionsSubView.OnFinishedAttackAnimation += InvokeOnAttackAnimationEnded;
        }

        private void DeregisterEvents()
        {
            _animaionsSubView.OnDo_StepForwardAnimationEvent -= InvokeDoStepForward;
            _animaionsSubView.OnFinished_MeleeSingleAttack -= InvokeOnAttackAnimationEnded;
            _animaionsSubView.OnFinished_MeleeTripleAttack -= InvokeOnAttackAnimationEnded;
            _animaionsSubView.OnFinishedAttackAnimation -= InvokeOnAttackAnimationEnded;
        }

        #endregion

        #region Attacks & Animations

        private void SetAnimationsSpeed()
        {
            _magicalAttackSpeed = Model.GetAttackSpeedForMagicalAttackAnimations();

            _playerAnimator.SetFloat(_movementAnimSpeedParam, Model.GetMovementSpeedForMoveAnimation());
            _playerAnimator.SetFloat(_attackAnimSpeedParam, Model.GetAttackSpeedForPhysicalAttackAnimations());
            _playerAnimator.SetFloat(_magicalAttackAnimSpeedParam, _magicalAttackSpeed);
        }

        private void SetIsMovingAnimation()
        {
            _playerAnimator.SetBool(_isMovingBool, Model.IsMoving);
        }

        private void SetIsTalkingAnimation()
        {
            _playerAnimator.SetBool(_isTalkingBool, Model.IsTalking);
        }

        private void HandleAttackAnimations()
        {
            if(!Model.IsAttacking)
                return;

            if(!Model.IsUsingSkill)
            {
                DoMeleeSingleAttack();
                return;
            }

            HandleDoSkill();
        }

        private async void HandleDoSkill()
        {
            switch(Model.ActiveSkill.SkillType)
            {
                case SkillType.MeleeAttack:

                    DoMeleeTripleAttack();
                    break;

                case SkillType.Meteor:

                    await DoMeteorSkill();
                    break;

                case SkillType.Teleport:

                    DoSummonPortal();
                    InvokeOnAttackAnimationEnded();
                    break;

                case SkillType.LightningShield:

                    DoLightningShield();
                    InvokeOnAttackAnimationEnded();
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

        private async Task DoMeteorSkill()
        {
            _playerAnimator.SetTrigger(_castSpellMeteorTrigger);

            var delay = (int)(_meteorSkillWaitInMilliSeconds / _magicalAttackSpeed);
            await Task.Delay(delay);

            _vfxView.SetIsPlayingEffect(VfxType.RedExplosion, true);
        }

        private void DoSummonPortal()
        {
            _vfxView.SetIsPlayingEffect(VfxType.Portal, true);
        }

        private void DoLightningShield()
        {
            _vfxView.SetIsPlayingEffect(VfxType.LightningShield, true);
        }

        private void InvokeOnAttackAnimationEnded()
        {
            OnAttackAnimationEnded?.Invoke();
        }

        #endregion

        #region HP & Hit Effect

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

