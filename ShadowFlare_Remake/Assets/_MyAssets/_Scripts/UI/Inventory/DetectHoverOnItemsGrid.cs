using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowFlareRemake.UI.Inventory {

    [RequireComponent(typeof(ItemsGrid))]
    public class DetectHoverOnItemsGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        [Header("References")]
        [SerializeField] private UIController _uiController;

        private ItemsGrid _currentHovereditemsGrid;

        private void Awake() {

            _currentHovereditemsGrid = GetComponent<ItemsGrid>();
        }

        public void OnPointerEnter(PointerEventData eventData) {

            _uiController.CurrentHoveredItemsGrid = _currentHovereditemsGrid;
        }

        public void OnPointerExit(PointerEventData eventData) {

            _uiController.CurrentHoveredItemsGrid = null;
        }
    }
}


