using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    public ItemsGrid CurrentHoveredItemsGrid;

    private void OnEnable()
    {
        PlayerController.OnLeftMouseButtonPressed += LeftMouseButtonWasPressed;
    }

    private void OnDisable()
    {
        PlayerController.OnLeftMouseButtonPressed += LeftMouseButtonWasPressed;
    }

    private void LeftMouseButtonWasPressed()
    {
        if (CurrentHoveredItemsGrid == null)
            return;

        print(CurrentHoveredItemsGrid.name + " was pressed at: " + CurrentHoveredItemsGrid.GetTileGridPosition(Mouse.current.position.ReadValue()));
    }
}
