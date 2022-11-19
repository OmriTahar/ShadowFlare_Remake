using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemsGrid))]
public class DetectHoverOnItemsGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InventoryController _inventoryController;
    private ItemsGrid _currentHovereditemsGrid;

    private void Awake()
    {
        _currentHovereditemsGrid = GetComponent<ItemsGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryController.CurrentHoveredItemsGrid = _currentHovereditemsGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inventoryController.CurrentHoveredItemsGrid = null;
    }
}
