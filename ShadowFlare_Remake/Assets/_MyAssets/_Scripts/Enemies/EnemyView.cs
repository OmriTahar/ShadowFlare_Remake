using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Tools;
using ShadowFlareRemake.VFX;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.Enemies
{
    public class EnemyView : View<BaseEnemyModel>
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<GameObject> OnParticleCollisionEvent;
        public event Action OnAttackAnimationEnded;
        public event Action OnDeath;
        public event Action OnFinishedDeathAnimation;
        public event Action OnFinishedFadeOutAnimation;

        public string Name { get => Model.Name; }
        public int CurrentHP { get => _lastSeenHP; }
        public int MaxHP { get => Model.Stats.MaxHP; }
        public float ScaleMultiplier { get => Model.Stats.ScaleMultiplier; }
        public int EvolutionLevel { get => Model.Stats.EvolutionLevel; }

        [Header("Collision & Renderers")]
        [SerializeField] private Collider _myCollider;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

        [Header("Animations & Effects")]
        [SerializeField] private Animator _animator;
        [SerializeField] private VisualEffectsSubView _vfxView;
        [SerializeField] private FadingObject _fadingObject;

        [Header("Settings")]
        [SerializeField] private bool _useSkinnedMeshRenderer;

        private int _lastSeenHP;
        private bool _isDying;

        private readonly int _evolutionLevelAnimHash = Animator.StringToHash("Evolution Level");
        private readonly int _isMovingAnimHash = Animator.StringToHash("Is Moving");
        private readonly int _attackAnimHash = Animator.StringToHash("Attack");
        private readonly int _dieAnimHash = Animator.StringToHash("Die");

        #region View Overrides

        protected override void Initialize()
        {
            CacheNulls();
        }

        protected override void ModelReplaced()
        {
            if(Model == null)
                return;

            SetColor();
            SetScale();
            SetEvolutionLevelAnimParam();
        }

        protected override void ModelChanged()
        {
            if(Model == null)
                return;

            HandleHitEffect();
            HandleHP();
            HandleEnemyState();
        }

        #endregion

        #region MonoBehaviour

        private void OnTriggerEnter(Collider other)
        {
            if(Model.CurrentEnemyState == EnemyState.Dead)
                return;

            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnParticleCollision(GameObject other)
        {
            OnParticleCollisionEvent?.Invoke(other);
        }

        #endregion

        #region Initialization

        private void SetColor()
        {
            if(_useSkinnedMeshRenderer)
            {
                _skinnedMeshRenderer.material.color = Model.Color;
                return;
            }

            _meshRenderer.material.color = Model.Color;
        }

        private void SetScale()
        {
            var currentLocalScale = transform.localScale.x;
            float scaleMultiplier = Model.Stats.ScaleMultiplier;
            float newScale = currentLocalScale * scaleMultiplier;
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        private void SetEvolutionLevelAnimParam()
        {
            _animator.SetInteger(_evolutionLevelAnimHash, Model.Stats.EvolutionLevel);
        }

        private void CacheNulls()
        {
            if(_myCollider == null)
            {
                _myCollider = GetComponent<Collider>();
            }
        }

        #endregion

        #region Meat & Potatos

        private void HandleHitEffect()
        {
            if(Model.Unit.CurrentHP >= _lastSeenHP || Model.CurrentEnemyState == EnemyState.Dead)
                return;

            if(Model.IsReceivedCritialHit)
            {
                _vfxView.SetIsPlayingEffect(VfxType.HitBlood, true);
                return;
            }

            _vfxView.SetIsPlayingEffect(VfxType.Hit, true);
        }

        private void HandleHP()
        {
            if(Model.CurrentEnemyState == EnemyState.Dead)
                return;

            var realHP = Model.Unit.CurrentHP;
            _lastSeenHP = realHP < 0 ? 0 : realHP;

            if(_lastSeenHP == 0)
            {
                OnDeath?.Invoke();
            }
        }

        private void HandleEnemyState()
        {
            switch(Model.CurrentEnemyState)
            {
                case EnemyState.Idle:
                    HandleIdleState();
                    break;

                case EnemyState.Chasing:
                    HandleChasingState();
                    break;

                case EnemyState.Attacking:
                    HandleAttackAnimations();
                    break;

                case EnemyState.Dead:
                    HandleDeath();
                    break;

                default:
                    break;
            }
        }

        private void HandleIdleState()
        {
            _animator.SetBool(_isMovingAnimHash, false);
        }

        private void HandleChasingState()
        {
            _animator.SetBool(_isMovingAnimHash, true);
        }

        private void HandleAttackAnimations()
        {
            if(!Model.IsAttacking)
                return;

            switch(Model.CurrentAttackRange)
            {
                case AttackRange.Close:
                    DoCloseAttack();
                    break;
            }
        }

        private void DoCloseAttack()
        {
            _animator.SetTrigger(_attackAnimHash);
        }

        public void FinishedAttackAnimation()
        {
            OnAttackAnimationEnded?.Invoke();
        }

        private void HandleDeath()
        {
            if(_isDying)
                return;

            _isDying = true;
            _animator.SetTrigger(_dieAnimHash);
            _vfxView.SetIsPlayingEffect(VfxType.DeathBlood, true);
            _myCollider.enabled = false;
        }

        public void FinishedDeathAnimation() // Called from an animation event 
        {
            FadeOut();
        }

        #endregion

        #region Helpers

        public float GetEnemyScaleMultiplier()
        {
            return Model.Stats.ScaleMultiplier;
        }

        #endregion

        #region Fade Out

        private const string _srcBlend_FadeOutParam = "_SrcBlend";
        private const string _dstBlend_FadeOutParam = "_DstBlend";
        private const string _zWrite_FadeOutParam = "_ZWrite";
        private const string _surface_FadeOutParam = "_Surface";
        private const string _deapthOnly_FadeOutParam = "DepthOnly";
        private const string _showCaster_FadeOutParam = "SHADOWCASTER";
        private const string _renderType_FadeOutParam = "RenderType";
        private const string _transparent_FadeOutParam = "Transparent";
        private const string _surfaceTypeTransparent = "_SURFACE_TYPE_TRANSPARENT";
        private const string _alphaPreMultiplyOn = "_ALPHAPREMULTIPLY_ON";
        private const string _baseColor = "_BaseColor";

        private void FadeOut()
        {
            StartCoroutine(FadeOutLogic(_fadingObject));
        }

        private IEnumerator FadeOutLogic(FadingObject fadingObject)
        {
            foreach(var material in fadingObject.Materials)
            {
                material.SetInt(_srcBlend_FadeOutParam, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt(_dstBlend_FadeOutParam, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt(_zWrite_FadeOutParam, 0);
                material.SetInt(_surface_FadeOutParam, 1);

                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                material.SetShaderPassEnabled(_deapthOnly_FadeOutParam, false);
                material.SetShaderPassEnabled(_showCaster_FadeOutParam, true);

                material.SetOverrideTag(_renderType_FadeOutParam, _transparent_FadeOutParam);

                material.EnableKeyword(_surfaceTypeTransparent);
                material.EnableKeyword(_alphaPreMultiplyOn);
            }

            float time = 0;
            float fadingDuration = _fadingObject.FadingDuration;

            if(fadingObject == null || fadingObject.Materials == null)
            {
                yield break;
            }

            while(fadingObject.Materials[0].color.a > 0)
            {
                foreach(var material in fadingObject.Materials)
                {
                    if(material.HasProperty(_baseColor))
                    {
                        material.color = new Color(material.color.r, material.color.g, material.color.b, 
                                                   Mathf.Lerp(fadingObject.InitialAlpha, 0, fadingDuration));
                    }
                }

                time += Time.deltaTime;
                yield return null;
            }

            StopAllCoroutines();
            OnFinishedFadeOutAnimation?.Invoke();
        }

        #endregion

     
    }
}


