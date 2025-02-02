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

        [Header("Health Sliders")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private MultiStateView _healthSlider_MSV;
        [SerializeField] private Slider _health_RestoreSlider;
        [SerializeField] private Slider _health_ReduceSlider;

        [Header("Mana Sliders")]
        [SerializeField] private Slider _manaSlider;
        [SerializeField] private MultiStateView _manaSlider_MSV;
        [SerializeField] private Slider _mana_RestoreSlider;
        [SerializeField] private Slider _manaRecudeSlider;

        [Header("EXP Sliders")]
        [SerializeField] private Slider _expSlider;

        [Header("Skills")]
        [SerializeField] private GameObject _skillsBar;
        [SerializeField] private int _skillsBarSidePosition_X;
        [SerializeField] private int _skillsBarPosition_Y;
        [SerializeField] private List<SkillUIView> _skillViews;

        private const float _sliderLerpDuration = 1.5f;

        private Coroutine _lastHealthCoroutine;
        private Coroutine _lastManaCoroutine;
        private float _lastSeenHealth;
        private float _lastSeenMana;
        private SkillsBarPosition _lastSkillBarPosition;

        #region View Overrides

        protected override void ModelReplaced()
        {
            _hudPanel.SetActive(true);
            InitHealthSliders();
            InitManaSliders();
            InitSkillsBar();
        }

        protected override void ModelChanged()
        {
            SetCloseButton();
            SetSkillsBarPosition();
            SetHealth();
            SetMana();
            SetExp();
            SetLevel();
        }

        protected override void Clean()
        {
            DeregisterFromSkillViewsEvents();
        }

        #endregion

        #region Initialization

        private void InitHealthSliders()
        {
            SetVitalSlidersMaxValue(_healthSlider, _health_RestoreSlider, _health_ReduceSlider, true);
            _healthSlider.value = Model.CurrentHP;
            _lastSeenHealth = _healthSlider.value;
            _healthSlider_MSV.ChangeState((int)Model.CurrentHpEffectSlider);
        }

        private void InitManaSliders()
        {
            SetVitalSlidersMaxValue(_manaSlider, _mana_RestoreSlider, _manaRecudeSlider, false);
            _manaSlider.value = Model.CurrentMP;
            _lastSeenMana = _manaSlider.value;
            _manaSlider_MSV.ChangeState((int)Model.CurrentMpEffectSlider);
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

        private void SetHealth()
        {
            SetVitalSlidersMaxValue(_healthSlider,_health_RestoreSlider,_health_ReduceSlider, true);
            StopLastVitalCoroutine(_lastHealthCoroutine);
            _healthSlider_MSV.ChangeState((int)Model.CurrentHpEffectSlider);

            switch(Model.CurrentHpEffectSlider)
            {
                case SliderEffectType.Restore:
                    HandleRestoreEffect(_health_RestoreSlider, _healthSlider, _lastHealthCoroutine, Model.CurrentHP, true);
                    break;

                case SliderEffectType.Reduce:
                    HandleReduceEffect(_health_ReduceSlider, _healthSlider, _lastHealthCoroutine, Model.CurrentHP, true);
                    break;

                default:
                    break;
            }
        }

        private void SetMana()
        {
            SetVitalSlidersMaxValue(_manaSlider, _mana_RestoreSlider, _manaRecudeSlider, false);
            StopLastVitalCoroutine(_lastManaCoroutine);
            _manaSlider_MSV.ChangeState((int)Model.CurrentMpEffectSlider);

            switch(Model.CurrentMpEffectSlider)
            {
                case SliderEffectType.Restore:
                    HandleRestoreEffect(_mana_RestoreSlider, _manaSlider, _lastManaCoroutine, Model.CurrentMP, false);
                    break;

                case SliderEffectType.Reduce:
                    HandleReduceEffect(_manaRecudeSlider, _manaSlider, _lastManaCoroutine, Model.CurrentMP, false);
                    break;

                default:
                    break;
            }
        }


        private void SetVitalSlidersMaxValue(Slider vitalSlider, Slider restoreSlider, Slider reduceSlider, bool isHealthSlider)
        {
            var amount = isHealthSlider ? Model.MaxHP : Model.MaxMP;

            vitalSlider.maxValue = amount;
            restoreSlider.maxValue = amount;
            reduceSlider.maxValue = amount;
        }

        private void StopLastVitalCoroutine(Coroutine lastCoroutine)
        {
            if(lastCoroutine != null)
            {
                StopCoroutine(lastCoroutine);
            }
        }

        private void HandleRestoreEffect(Slider restoreSlider, Slider vitalSlider, Coroutine lastCoroutine, float sliderTargetValue, bool isHealthSlider)
        {
            restoreSlider.value = sliderTargetValue;
            vitalSlider.value = isHealthSlider? _lastSeenHealth : _lastSeenMana;
            lastCoroutine = StartCoroutine(LerpSliderValue(vitalSlider, sliderTargetValue, isHealthSlider));
        }

        private void HandleReduceEffect(Slider reduceSlider, Slider vitalSlider, Coroutine lastCoroutine, float sliderTargetValue, bool isHealthSlider)
        {
            vitalSlider.value = sliderTargetValue;
            reduceSlider.value = isHealthSlider ? _lastSeenHealth : _lastSeenMana;
            lastCoroutine = StartCoroutine(LerpSliderValue(reduceSlider, sliderTargetValue, isHealthSlider));
        }

        public IEnumerator LerpSliderValue(Slider slider, float targetValue, bool isHealthSlider)
        {
            float startValue = slider.value;
            float timeElapsed = 0f;

            while(timeElapsed < _sliderLerpDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpProgress = timeElapsed / _sliderLerpDuration;
                slider.value = Mathf.Lerp(startValue, targetValue, lerpProgress);

                if(isHealthSlider)
                    _lastSeenHealth = slider.value;
                else
                    _lastSeenMana = slider.value;

                yield return null;
            }

            slider.value = targetValue;

            if(isHealthSlider)
                _lastSeenHealth = slider.value;
            else
                _lastSeenMana = slider.value;
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

        private void DeregisterFromSkillViewsEvents()
        {
            foreach(var view in _skillViews)
            {
                view.OnSkillClicked -= InvokeSkillItemClicked;
            }
        }

        #endregion
    }
}

