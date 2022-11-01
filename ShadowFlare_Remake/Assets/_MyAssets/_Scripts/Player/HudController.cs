using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    public GameObject _inventoryPanel;
    public GameObject _hudPanel;


    private bool _isInventoryOpen = false;

    private void Start()
    {
        _hudPanel.SetActive(true);
        _inventoryPanel.SetActive(false);

        // ---------------- YOU STOPPED HERE! --------------------
        //_playerController.OnToggleInventory += ToggleInventory;
    }

    public void ToggleInventory()
    {
        _inventoryPanel.SetActive(!_isInventoryOpen);
        _isInventoryOpen = !_isInventoryOpen;
    }
}
