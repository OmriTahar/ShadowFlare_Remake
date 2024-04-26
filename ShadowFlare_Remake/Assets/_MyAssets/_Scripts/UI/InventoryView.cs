using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI {
    public class InventoryView : View<InventoryModel>, IPointerEnterHandler, IPointerExitHandler {

        public event Action<PointerEventData> OnCurserEnterUI;
        public event Action<PointerEventData> OnCurserLeftUI;
        public event Action OnCloseClicked;

        [Header("References")]
        [SerializeField] private GameObject _inventoryPanel;

        protected override void ModelChanged() {
            _inventoryPanel.SetActive(Model.IsInventoryOpen);
        }

        public void CloseClicked() {
            OnCloseClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnCurserEnterUI?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnCurserLeftUI?.Invoke(eventData);
        }
    }

}
