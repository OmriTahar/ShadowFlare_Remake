using ShadowFlareRemake.Skills;
using ShadowFlareRemake.Tools;
using ShadowFlareRemake.UI.Skills;
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
        public event Action<ISkillData> OnSkillItemClicked;
        public event Action OnInventoryButtonClicked;
        public event Action OnStatsClicked;

        [Header("General")]
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
        [SerializeField] private MultiStateView _mpSlider_MSV;
        [SerializeField] private Slider _mpHealSlider;
        [SerializeField] private Slider _mpHitSlider;

        [Header("EXP Sliders")]
        [SerializeField] private Slider _expSlider;

        [Header("Skills")]
        [SerializeField] private GameObject _skillsBar;
        [SerializeField] private int _skillsBarSidePosition_X;
        [SerializeField] private int _skillsBarPosition_Y;
        [SerializeField] private List<SkillUIView> _skillViews;

        private const float _sliderLerpDuration = 1.5f;

        private Coroutine _lastHpCoroutine;
        private Coroutine _lastMpCoroutine;
        private float _lastSeenHP;
        private float _lastSeenMP;
        private SkillsBarPosition _lastSkillBarPosition;

        #region View Overrides

        protected override void ModelReplaced()
        {
            _hudPanel.SetActive(true);
            InitHpSliders();
            InitMpSliders();
            InitSkillsBar();
        }

        protected override void ModelChanged()
        {
            SetCloseButton();
            SetSkillsBarPosition();
            SetHP();
            SetMP();
            SetExp();
            SetLevel();
        }

        protected override void Clean()
        {
            foreach(var view in _skillViews)
            {
                view.OnSkillClicked -= InvokeSkillItemClicked;
            }
        }

        #endregion

        #region Initialization

        private void InitHpSliders()
        {
            SetVitalsSlidersMaxValue(_hpSlider, _hpHealSlider, _hpHitSlider);
            _hpSlider.value = Model.CurrentHP;
            _lastSeenHP = _hpSlider.value;
            _hpSlider_MSV.ChangeState((int)Model.CurrentHpEffectSlider);
        }

        private void InitMpSliders()
        {
            SetVitalsSlidersMaxValue(_mpSlider, _mpHealSlider, _mpHitSlider);
            _mpSlider.value = Model.CurrentMP;
            _lastSeenMP = _mpSlider.value;
            _mpSlider_MSV.ChangeState((int)Model.CurrentMpEffectSlider);
        }

        private void InitSkillsBar()
        {
            for(int i = 0; i < _skillViews.Count; i++)
            {
                _skillViews[i].SetModel(Model.SkillModels[i]);
                _skillViews[i].OnSkillClicked += InvokeSkillItemClicked;
            }
        }

        #endregion

        #region Meat & Potatos

        private void SetCloseButton()
        {
            _closeButton.SetActive(Model.IsCloseButtonActive);
        }

        private void SetSkillsBarPosition()
        {
            if(_lastSkillBarPosition == Model.CurrentSkillsBarPosition)
                return;

            var newPos = new Vector3(0, _skillsBarPosition_Y, 0);

            switch(Model.CurrentSkillsBarPosition)
            {
                case SkillsBarPosition.Middle:

                    _skillsBar.transform.localPosition = newPos;
                    _skillsBar.SetActive(true);
                    break;

                case SkillsBarPosition.Left:

                    newPos.x = -_skillsBarSidePosition_X;
                    _skillsBar.transform.localPosition = newPos;
                    _skillsBar.SetActive(true);
                    break;

                case SkillsBarPosition.Right:

                    newPos.x = _skillsBarSidePosition_X;
                    _skillsBar.transform.localPosition = newPos;
                    _skillsBar.SetActive(true);
                    break;

                case SkillsBarPosition.None:

                    _skillsBar.SetActive(false);
                    break;
            }

            _lastSkillBarPosition = Model.CurrentSkillsBarPosition;
        }

        private void SetHP()
        {
            SetVitalsSlidersMaxValue(_hpSlider,_hpHealSlider,_hpHitSlider);
            StopLastVitalCoroutine(_lastHpCoroutine);
            _hpSlider_MSV.ChangeState((int)Model.CurrentHpEffectSlider);

            switch(Model.CurrentHpEffectSlider)
            {
                case SliderEffectType.Fill:
                    HandleHealEffect(_hpHealSlider, _hpSlider, _lastHpCoroutine, Model.CurrentHP,true);
                    break;

                case SliderEffectType.Reduce:
                    HandleHitEffect(_hpHitSlider, _hpSlider, _lastHpCoroutine, Model.CurrentHP, true);
                    break;

                default:
                    break;
            }
        }

        private void SetMP()
        {
            SetVitalsSlidersMaxValue(_mpSlider, _mpHealSlider, _mpHitSlider);
            StopLastVitalCoroutine(_lastMpCoroutine);
            _mpSlider_MSV.ChangeState((int)Model.CurrentMpEffectSlider);

            switch(Model.CurrentMpEffectSlider)
            {
                case SliderEffectType.Fill:
                    HandleHealEffect(_mpHealSlider, _mpSlider, _lastMpCoroutine, Model.CurrentMP, false);
                    break;

                case SliderEffectType.Reduce:
                    HandleHitEffect(_mpHitSlider, _mpSlider, _lastMpCoroutine, Model.CurrentMP, false);
                    break;

                default:
                    break;
            }
        }


        private void SetVitalsSlidersMaxValue(Slider vitalSlider, Slider healSlider, Slider hitSlider)
        {
            vitalSlider.maxValue = Model.MaxHP;
            healSlider.maxValue = Model.MaxHP;
            hitSlider.maxValue = Model.MaxHP;
        }

        private void StopLastVitalCoroutine(Coroutine lastCoroutine)
        {
            if(lastCoroutine != null)
            {
                StopCoroutine(lastCoroutine);
            }
        }

        private void HandleHealEffect(Slider healSlider, Slider vitalSlider, Coroutine lastCoroutine, float sliderTargetValue, bool isHP)
        {
            healSlider.value = Model.CurrentHP;
            vitalSlider.value = _lastSeenHP;
            lastCoroutine = StartCoroutine(LerpSliderValue(vitalSlider, sliderTargetValue, isHP));
        }

        private void HandleHitEffect(Slider hitSlider, Slider vitalSlider, Coroutine lastCoroutine, float sliderTargetValue, bool isHP)
        {
            vitalSlider.value = Model.CurrentHP;
            hitSlider.value = _lastSeenHP;
            lastCoroutine = StartCoroutine(LerpSliderValue(hitSlider, sliderTargetValue, isHP));
        }

        public IEnumerator LerpSliderValue(Slider slider, float targetValue, bool isHpSlider)
        {
            float startValue = slider.value;
            float timeElapsed = 0f;

            while(timeElapsed < _sliderLerpDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpProgress = timeElapsed / _sliderLerpDuration;
                slider.value = Mathf.Lerp(startValue, targetValue, lerpProgress);

                if(isHpSlider)
                    _lastSeenHP = slider.value;
                else
                    _lastSeenHP = slider.value;

                yield return null;
            }

            if(isHpSlider)
                _lastSeenHP = slider.value;
            else
                _lastSeenHP = slider.value;

            slider.value = targetValue;
            _lastSeenHP = slider.value;
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

        #region Helpers

        public List<SkillUIView> GetSkillUIViews()
        {
            return _skillViews;
        }

        #endregion

        #region Events

        private void InvokeSkillItemClicked(ISkillData skillData)
        {
            OnSkillItemClicked?.Invoke(skillData);
        }

        #endregion
    }
}

