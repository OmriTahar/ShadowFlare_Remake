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

        private readonly int _isMovingAnimHash = Animator.StringToHash("Is Moving");
        private readonly int _deathAnimHash = Animator.StringToHash("Death");

        #region View Overrides

        protected override void Initialize()
        {
            CacheNulls();
        }

        protected override void ModelReplaced()
        {
            if(Model == null)
                return;

            SetNameText();
            SetColor();
            ResetHealthSliderValues();
        }

        protected override void ModelChanged()
        {
            if(Model == null || Model.CurrentEnemyState == EnemyState.Dead)
                return;

            HandleHitEffect();
            HandleHP();

            if(Model.IsAttacking)
                HandleAttackAnimations();
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

        private void ResetHealthSliderValues()
        {
            var value = Model.Stats.MaxHP;
            _healthSlider.maxValue = value;
            _healthSlider.value = value;
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
            if(_hitEffect == null)
            {
                Debug.LogError("Hit effect is null");
                return;
            }

            if(_lastSeenHP > Model.Unit.CurrentHP)
            {
                if(_hitEffect.isPlaying)
                {
                    _hitEffect.Stop();
                }
                _hitEffect.Play();
            }
        }

        private void HandleHP()
        {
            var hp = Model.Unit.CurrentHP;
            _lastSeenHP = hp < 0 ? 0 : hp;
            _healthSlider.value = _lastSeenHP;

            if(_lastSeenHP == 0)
            {
                OnDeath?.Invoke();
                HandleDeath();
            }
        }

        private void HandleEnemyState()
        {
            switch(Model.CurrentEnemyState)
            {
                case EnemyState.Idle:
                    _animator.SetBool(_isMovingAnimHash, false);
                    break;

                case EnemyState.Chasing:
                    _animator.SetBool(_isMovingAnimHash, true);
                    break;

                case EnemyState.Attacking:
                    HandleAttackAnimations();
                    break;

                case EnemyState.Dead:
                    break;

                default:
                    break;
            }
        }

        private void HandleAttackAnimations()
        {
            switch(Model.CurrentAttackMethod)
            {
                case AttackMethod.Close:
                    DoCloseAttack();
                    break;
            }
        }

        private void DoCloseAttack()
        {
            _animator.SetTrigger("CloseAttack");
        }

        private void HandleDeath()
        {
            _animator.SetTrigger(_deathAnimHash);
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

        private void FadeOut()
        {
            StartCoroutine(FadeOutLogic(_fadingObject));
        }

        private IEnumerator FadeOutLogic(FadingObject fadingObject)
        {
            foreach(var material in fadingObject.Materials)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.SetInt("_Surface", 1);

                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                material.SetShaderPassEnabled("DepthOnly", false);
                material.SetShaderPassEnabled("SHADOWCASTER", true);

                material.SetOverrideTag("RenderType", "Transparent");

                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
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
                    if(material.HasProperty("_BaseColor"))
                    {

                        material.color = new Color(
                            material.color.r,
                            material.color.g,
                            material.color.b,
                            Mathf.Lerp(fadingObject.InitialAlpha, 0, time * 2.5f)
                            );
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


