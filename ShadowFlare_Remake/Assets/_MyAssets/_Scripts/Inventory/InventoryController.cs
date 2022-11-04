using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemsGrid _selectedItemGrid;

    private void Update()
    {
        if (_selectedItemGrid == null)
            return;

        Debug.Log(_selectedItemGrid.GetTileGridPosition(Mouse.current.position.ReadValue()));
    }
}
