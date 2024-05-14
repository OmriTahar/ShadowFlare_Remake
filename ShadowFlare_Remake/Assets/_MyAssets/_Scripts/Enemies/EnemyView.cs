using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ShadowFlareRemake.Tools;
using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Enemies {
    public class EnemyView : View<EnemyModel> {

        public event Action<Collider> OnTriggerEnterEvent;
        public event Action OnAttackAnimationEnded;
        public event Action OnEnemyKilled;
        public event Action OnFinishedDeathAnimation;

        [Header("Health Slider")]
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Transform _healthSliderTransform;

        [Header("Other")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _myCollider;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private FadingObject _fadingObject;

        [Header("Animations")]
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _closeAttackAnim;
        [SerializeField] private AnimationClip _rangedAttackAnim;
        [SerializeField] private AnimationClip _deathAnim;

        private WaitForSeconds _closeAttackCooldown;

        private float _closeAttackAnimLength;
        private float _rangedAttackAnimLength;
        private float _deathAnimLength;

        private int _lastSeenHP;

        protected override void Initialize() {

            CacheNulls();
            _closeAttackCooldown = new WaitForSeconds(_closeAttackAnimLength);
        }

        private void Update() {

            StabilizeHpSlider();
        }

        private void OnTriggerEnter(Collider other) {

            OnTriggerEnterEvent?.Invoke(other);
        }

        protected override void ModelReplaced() {

            if(Model == null) {
                return;
            }

            _name.text = Model.Name;

            var color = Model.Color;
            _meshRenderer.material.color = new Color(color.r, color.g, color.b, 1);

            ResetHealthSliderValues();
        }

        protected override void ModelChanged() {

            if(Model == null)
                return;

            _healthSlider.gameObject.SetActive(Model.IsEnemyHighlighted);

            HandleHitEffect();
            HandleHP();

            if(Model.IsAttacking) {
                HandleAttackAnimations();
            }
        }

        public Collider GetEnemyCollider() {

            return _myCollider;
        }

        private void ResetHealthSliderValues() {

            var value = Model.Stats.MaxHP;
            _healthSlider.maxValue = value;
            _healthSlider.value = value;
        }

        private void HandleHitEffect() {

            if(_hitEffect == null) {
                Debug.LogError("Hit effect is null");
                return;
            }

            if(_lastSeenHP > Model.Unit.CurrentHP) {

                if(_hitEffect.isPlaying) {
                    _hitEffect.Stop();
                }
                _hitEffect.Play();
            }
        }

        private void HandleHP() {

            var hp = Model.Unit.CurrentHP;
            _lastSeenHP = hp < 0 ? 0 : hp;
            _healthSlider.value = _lastSeenHP;

            if(_lastSeenHP == 0) {

                OnEnemyKilled?.Invoke();
                HandleDeathAnimation();
            }
        }

        public void HandleAttackAnimations() {

            switch(Model.CurrentAttackMethod) {

                case AttackMethod.Close:
                    DoCloseAttack();
                    break;
            }
        }

        private void DoCloseAttack() {

            _animator.SetTrigger("CloseAttack");
            StartCoroutine(WaitForAnimationEnd(Model.CurrentAttackMethod));
        }

        private async void HandleDeathAnimation() {

            print($"{Model.Name} was killed!");

            _animator.SetTrigger("Die");

            await Task.Delay((int)_deathAnimLength * 1000);

            FadeOut();
        }

        private IEnumerator WaitForAnimationEnd(AttackMethod attackMethod) {

            switch(attackMethod) {

                case AttackMethod.Close:
                    yield return _closeAttackCooldown;
                    break;

                case AttackMethod.Range:
                    break;

            }

            OnAttackAnimationEnded?.Invoke();
        }

        private void FadeOut() {
            StartCoroutine(FadeOutLogic(_fadingObject));
        }

        private void StabilizeHpSlider() {

            if(_healthSlider != null) {
                _healthSliderTransform.rotation = Quaternion.Euler(45, 45, 0);
            }
        }

        private void CacheNulls() {

            if(_myCollider == null) {
                _myCollider = GetComponent<Collider>();
            }

            if(_closeAttackAnim != null) {
                _closeAttackAnimLength = _closeAttackAnim.length;
            }

            if(_rangedAttackAnim != null) {
                _rangedAttackAnimLength = _rangedAttackAnim.length;
            }

            if(_deathAnim != null) {
                _deathAnimLength = _deathAnim.length;
            }
        }

        private IEnumerator FadeOutLogic(FadingObject fadingObject) {

            foreach(var material in fadingObject.Materials) {

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

            if(fadingObject == null || fadingObject.Materials == null) {
                yield break;
            }

            while(fadingObject.Materials[0].color.a > 0) {

                foreach(var material in fadingObject.Materials) {

                    if(material.HasProperty("_BaseColor")) {

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
            OnFinishedDeathAnimation?.Invoke();
        }
    }
}


