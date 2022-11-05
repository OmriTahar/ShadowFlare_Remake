using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    public ItemsGrid CurrentHoveredItemsGrid;

    //private void OnEnable()
    //{
    //    PlayerController.OnLeftMouseButtonPressed += leftMouseButtonWasPressed;
    //}

    //private void OnDisable()
    //{
    //    PlayerController.OnLeftMouseButtonPressed += leftMouseButtonWasPressed;
    //}

    private void Update()
    {
        if (CurrentHoveredItemsGrid == null)
            return;

        //if ()
        //{

        //}

        //print(SelectedItemsGrid.GetTileGridPosition(Mouse.current.position.ReadValue()));
    }

    //private void leftMouseButtonWasPressed()
    //{

    //}
}
