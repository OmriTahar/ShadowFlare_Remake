using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OnPointerEnteredUI;
    public static event Action OnPointerLeftUI;

    public GameObject _inventoryPanel;
    public GameObject _hudPanel;

    private bool _isInventoryOpen = false;

    private void Start()
    {
        PanelsInit();
    }

    private void OnEnable()
    {
        PlayerController.OnInventoryPressed += ToggleInventory;
    }

    private void OnDisable()
    {
        PlayerController.OnInventoryPressed -= ToggleInventory;
    }

    private void PanelsInit()
    {
        _hudPanel.SetActive(true);
        _inventoryPanel.SetActive(false);
        _isInventoryOpen = false;
    }

    private void ToggleInventory()
    {
        _inventoryPanel.SetActive(!_isInventoryOpen);
        _isInventoryOpen = !_isInventoryOpen;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnteredUI?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerLeftUI?.Invoke();
    }
}
