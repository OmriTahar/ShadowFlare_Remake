using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIController _uiController;

    [Header("Cameras")]
    [SerializeField] private GameObject _centeredCamera;
    [SerializeField] private GameObject _inventoryCamera;

    private bool _isInventoryCamera = false;

    void Start()
    {
        CamerasInit();
    }

    private void OnEnable()
    {
        PlayerController.OnInventoryPressed += ToggleInventoryCamera;
    }

    private void OnDisable()
    {
        PlayerController.OnInventoryPressed -= ToggleInventoryCamera;
    }

    private void CamerasInit()
    {
        _centeredCamera.SetActive(true);
        _inventoryCamera.SetActive(false);
        _isInventoryCamera = false;
    }

    private void ToggleInventoryCamera()
    {
        _inventoryCamera.SetActive(!_isInventoryCamera);
        _centeredCamera.SetActive(_isInventoryCamera);
        _isInventoryCamera = !_isInventoryCamera;
    }
}
