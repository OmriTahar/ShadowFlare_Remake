using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShadowFlareRemake.UI {
    public class HudView : View<HudModel>, IPointerEnterHandler, IPointerExitHandler {

        public event Action<PointerEventData> OnCurserEnterUI;
        public event Action<PointerEventData> OnCurserLeftUI;

        public event Action OnInventoryButtonClicked;
        public event Action OnStatsClicked;

        [Header("Game Objects")]
        [SerializeField] private GameObject _hudPanel;

        [Header("Texts")]
        [SerializeField] private TMP_Text _levelText; 

        [Header("Sliders")]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Slider _mpSlider;
        [SerializeField] private Slider _expSlider;

        protected override void ModelReplaced() {

            base.ModelReplaced();
            _hudPanel.SetActive(true);
        }

        protected override void ModelChanged() {

            SetHP();
            SetMP();
            SetExp();
            SetLevel();
        }

        private void SetHP() {

            _hpSlider.maxValue = Model.MaxHP;
            _hpSlider.value = Model.CurrentHP;
        }

        private void SetMP() {

            _mpSlider.maxValue = Model.MaxMP;
            _mpSlider.value = Model.CurrentMP; 
        }

        private void SetExp() {

            _expSlider.maxValue = Model.ExpToLevelUp;
            _expSlider.value = Model.CurrentExp;
        }

        private void SetLevel() {

            _levelText.text = Model.Level.ToString();
        }

        public void InventoryClicked() {

            OnInventoryButtonClicked?.Invoke();
        }

        public void StatsClicked() {

            OnStatsClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnCurserEnterUI?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnCurserLeftUI?.Invoke(eventData);
        }
    }
}

