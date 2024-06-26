using ShadowFlareRemake.Enums;
using ShadowFlareRemake.Tools;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.Enemies
{
    public class EnemyView : View<EnemyModel>
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action OnAttackAnimationEnded;
        public event Action OnDeath;
        public event Action OnFinishedDeathAnimation;
        public event Action OnFinishedFadeOutAnimation;

        [Header("Collision & Renderers")]
        [SerializeField] private Collider _myCollider;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

        [Header("Health Slider")]
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Transform _healthSliderTransform;

        [Header("Animations & Effects")]
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private ParticleSystem _bloodEffect;
        [SerializeField] private FadingObject _fadingObject;

        [Header("Settings")]
        [SerializeField] private Vector3 _healthBarStabilizer;
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

            ResetHealthSliderValues();
            SetNameText();
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

        private void FixedUpdate()
        {
            StabilizeHpSlider();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(Model.CurrentEnemyState == EnemyState.Dead)
                return;

            OnTriggerEnterEvent?.Invoke(other);
        }

        #endregion

        #region Initialization

        private void SetNameText()
        {
            _name.text = Model.Name;
        }

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
            float scale = Model.Stats.Scale;
            transform.localScale = new Vector3(scale, scale, scale);
        }

        private void SetEvolutionLevelAnimParam()
        {
            _animator.SetInteger(_evolutionLevelAnimHash, Model.Stats.EvolutionLevel);
        }

        private void ResetHealthSliderValues()
        {
            var value = Model.Stats.MaxHP;
            _healthSlider.maxValue = value;
            _healthSlider.value = value;
            _lastSeenHP = Model.Unit.CurrentHP;
        }

        public Collider GetEnemyCollider()
        {
            return _myCollider;
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
            if(_lastSeenHP == Model.Unit.CurrentHP || _hitEffect == null || Model.CurrentEnemyState == EnemyState.Dead)
                return;

            if(_hitEffect.isPlaying)
            {
                _hitEffect.Stop();
            }

            _hitEffect.Play();
        }

        private void HandleHP()
        {
            if(Model.CurrentEnemyState == EnemyState.Dead)
                return;

            var realHP = Model.Unit.CurrentHP;
            _lastSeenHP = realHP < 0 ? 0 : realHP;
            _healthSlider.value = _lastSeenHP;

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

            switch(Model.CurrentAttackMethod)
            {
                case AttackMethod.Close:
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
            _bloodEffect.Play();
            _myCollider.enabled = false;
        }

        public void FinishedDeathAnimation() // Called from an animation event 
        {
            FadeOut();
        }

        private void StabilizeHpSlider()
        {
            _healthSliderTransform.rotation = Quaternion.Euler(_healthBarStabilizer);
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
                        material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.Lerp(fadingObject.InitialAlpha, 0, time * 2.5f));
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


