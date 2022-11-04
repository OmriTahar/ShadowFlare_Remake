using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject _inventoryPanel;
    public GameObject _hudPanel;

    public bool IsInventoryOpen = false;

    private void Start()
    {
        InitPanels();
    }

    private void OnEnable()
    {
        PlayerController.OnInventoryPressed += ToggleInventory;
    }

    private void OnDisable()
    {
        PlayerController.OnInventoryPressed -= ToggleInventory;
    }

    private void InitPanels()
    {
        _hudPanel.SetActive(true);
        _inventoryPanel.SetActive(false);
        IsInventoryOpen = false;
    }

    private void ToggleInventory()
    {
        _inventoryPanel.SetActive(!IsInventoryOpen);
        IsInventoryOpen = !IsInventoryOpen;
    }
}
