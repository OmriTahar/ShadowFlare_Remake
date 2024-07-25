using ShadowFlareRemake.Tools;
using ShadowFlareRemake.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI.Hud
{
    public class HudView : UIView<HudModel>
    {
        public event Action OnInventoryButtonClicked;
        public event Action OnStatsClicked;

        [Header("Game Objects")]
        [SerializeField] private GameObject _hudPanel;
        [SerializeField] private GameObject _closeButton; 

        [Header("Texts")]
        [SerializeField] private TMP_Text _levelText;

        [Header("HP Sliders")]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private MultiStateView _hpSlider_MSV;
        [SerializeField] private Slider _hpHealSlider;
        [SerializeField] private Slider _hpHitSlider;

        [Header("MP Sliders")]
        [SerializeField] private Slider _mpSlider;

        [Header("EXP Sliders")]
        [SerializeField] private Slider _expSlider;

        [Header("Skills")]
        [SerializeField] private List<SkillView> _skillViews;

        private const float _sliderLerpDuration = 1.5f;

        private Coroutine _lastHpCoroutine;
        private float _lastSeenHP;

        #region View Overrides

        protected override void ModelReplaced()
        {
            _hudPanel.SetActive(true);
            InitHpSliders();
            InitSkillsBar();
        }

        protected override void ModelChanged()
        {
            SetCloseButton();
            SetHP();
            SetMP();
            SetExp();
            SetLevel();
        }

        #endregion

        #region Initialization

        private void InitHpSliders()
        {
            SetHpSlidersMaxValue();
            _hpSlider.value = Model.CurrentHP;
            _lastSeenHP = _hpSlider.value;
            _hpSlider_MSV.ChangeState((int)Model.CurrentHpEffectSlider);
        }

        private void InitSkillsBar()
        {
            for(int i = 0; i < _skillViews.Count; i++)
            {
                _skillViews[i].SetModel(Model.SkillModels[i]);
            }
        }

        #endregion

        #region Meat & Potatos

        private void SetCloseButton()
        {
            _closeButton.SetActive(Model.IsCloseButtonActive);
        }

        private void SetHP()
        {
            SetHpSlidersMaxValue();

            if(_lastHpCoroutine != null)
            {
                StopCoroutine(_lastHpCoroutine);
            }

            _hpSlider_MSV.ChangeState((int)Model.CurrentHpEffectSlider);

            switch(Model.CurrentHpEffectSlider)
            {
                case SliderEffectType.Fill:
                    HandleHpHealEffect();
                    break;

                case SliderEffectType.Reduce:
                    HandleHpHitEffect();
                    break;

                default:
                    break;
            }
        }

        private void SetHpSlidersMaxValue()
        {
            _hpSlider.maxValue = Model.MaxHP;
            _hpHealSlider.maxValue = Model.MaxHP;
            _hpHitSlider.maxValue = Model.MaxHP;
        }

        private void HandleHpHealEffect()
        {
            _hpHealSlider.value = Model.CurrentHP;
            _hpSlider.value = _lastSeenHP;
            _lastHpCoroutine = StartCoroutine(LerpSliderValue(_hpSlider, Model.CurrentHP));
        }

        private void HandleHpHitEffect()
        {
            _hpSlider.value = Model.CurrentHP;
            _hpHitSlider.value = _lastSeenHP;
            _lastHpCoroutine = StartCoroutine(LerpSliderValue(_hpHitSlider, Model.CurrentHP));
        }

        public IEnumerator LerpSliderValue(Slider slider, float targetValue)
        {
            float startValue = slider.value;
            float timeElapsed = 0f;

            while(timeElapsed < _sliderLerpDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpProgress = timeElapsed / _sliderLerpDuration;
                slider.value = Mathf.Lerp(startValue, targetValue, lerpProgress);
                _lastSeenHP = slider.value;
                yield return null;
            }

            slider.value = targetValue;
            _lastSeenHP = slider.value;
        }

        private void SetMP()
        {
            _mpSlider.maxValue = Model.MaxMP;
            _mpSlider.value = Model.CurrentMP;
        }

        private void SetExp()
        {
            _expSlider.maxValue = Model.ExpToLevelUp;
            _expSlider.value = Model.CurrentExp;
        }

        private void SetLevel()
        {
            _levelText.text = Model.Level.ToString();
        }

        #endregion

        #region Buttons

        public void InventoryClicked() // Called from a UI button clicked event
        {
            OnInventoryButtonClicked?.Invoke();
        }

        public void StatsClicked() // Called from a UI button clicked event
        {
            OnStatsClicked?.Invoke();
        }

        #endregion
    }
}

