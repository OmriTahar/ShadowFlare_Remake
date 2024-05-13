using System;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI {
    public abstract class UIView<T> : View<T>, IPointerEnterHandler, IPointerExitHandler where T : class, IModel { 

        public event Action<PointerEventData> OnCurserEnterUI;
        public event Action<PointerEventData> OnCurserLeftUI;

        protected override void ModelChanged() { }

        public void OnPointerEnter(PointerEventData eventData) {

            print("Enter UI. Name: " + name);
            OnCurserEnterUI?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {

            print("Left UI. Name: " + name);
            OnCurserLeftUI?.Invoke(eventData);
        }
    }
}
