using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("References")]
    public Texture2D[] CursorIconsArray;
    [SerializeField] private Player _player;
    [SerializeField] private Camera _mainCamera;

    [Header("Input Actions")]
    [SerializeField] private InputAction _leftMouseClickAction;
    [SerializeField] private InputAction _iKeyboardClickAction;

    private Dictionary<CursorIconState, Texture2D> _cursorsIconsDictionary;
    private CursorIconState _currentCursorIconState;
    private bool _isCursorOnUI = false;

    private CharacterController _characterController;
    private Ray _currentMouseRay;
    private Vector3 _targetPos;
    private Coroutine _lastMoveCoroutine;

    private int _groundLayer;
    private int _enemyLayer;
    private int _itemLayer;

    #region Events

    public static event Action OnInventoryPressed;

    #endregion

    private enum CursorIconState
    {
        Move,
        Attack,
        CollectItem,
        UI
    }

    #region Callbacks

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        CursorIconsInit();
        LayersInit();
    }

    private void OnEnable()
    {
        UIController.OnPointerEnteredUI += CursorEnteredUI;
        UIController.OnPointerLeftUI += CursorLeftUI;

        _leftMouseClickAction.Enable();
        _leftMouseClickAction.performed += Move;
        _iKeyboardClickAction.Enable();
        _iKeyboardClickAction.performed += ToggleInventory;
    }

    private void OnDisable()
    {
        UIController.OnPointerEnteredUI -= CursorEnteredUI;
        UIController.OnPointerLeftUI -= CursorLeftUI;

        _leftMouseClickAction.performed -= Move;
        _leftMouseClickAction.Disable();
        _iKeyboardClickAction.performed -= ToggleInventory;
        _iKeyboardClickAction.Disable();
    }

    private void Update()
    {
        UpdateCursorIcon();
    }

    #endregion

    #region Cursor Functions

    private void LayersInit()
    {
        _groundLayer = LayerMask.NameToLayer("Ground");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _itemLayer = LayerMask.NameToLayer("Item");
    }

    private void CursorIconsInit()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(CursorIconsArray[0], Vector2.zero, CursorMode.Auto);
        _currentCursorIconState = CursorIconState.Move;

        _cursorsIconsDictionary = new Dictionary<CursorIconState, Texture2D>()
        {
            { CursorIconState.Move, CursorIconsArray[0] },
            { CursorIconState.Attack, CursorIconsArray[1] } ,
            { CursorIconState.CollectItem, CursorIconsArray[2] } ,
            { CursorIconState.UI, CursorIconsArray[3] }
        };
    }

    private void UpdateCursorIcon()
    {
        if (!_isCursorOnUI)
        {
            _currentMouseRay = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(_currentMouseRay, out RaycastHit hit))
            {
                if (hit.collider)
                {
                    if (hit.collider.gameObject.layer.CompareTo(_groundLayer) == 0)
                        ChangeCursor(CursorIconState.Move);

                    else if (hit.collider.gameObject.layer.CompareTo(_enemyLayer) == 0)
                        ChangeCursor(CursorIconState.Attack);

                    else if (hit.collider.gameObject.layer.CompareTo(_itemLayer) == 0)
                        ChangeCursor(CursorIconState.CollectItem);
                }
            }
        }
        else
        {
            ChangeCursor(CursorIconState.UI);
        }
    }

    private void ChangeCursor(CursorIconState newCursorState)
    {
        if (_currentCursorIconState == newCursorState)
            return;

        Cursor.SetCursor(_cursorsIconsDictionary[newCursorState], Vector2.zero, CursorMode.Auto);
        _currentCursorIconState = newCursorState;
    }

    private void CursorEnteredUI()
    {
        _isCursorOnUI = true;
    }

    private void CursorLeftUI()
    {
        _isCursorOnUI = false;
    }

    #endregion

    #region Player Input

    #region Mouse Input

    private void Move(InputAction.CallbackContext context)
    {
        if (!_isCursorOnUI)
        {
            if (Physics.Raycast(_currentMouseRay, out RaycastHit hit) && hit.collider)
            {
                if (hit.collider.gameObject.layer.CompareTo(_groundLayer) == 0)
                {
                    if (_lastMoveCoroutine != null)
                        StopCoroutine(_lastMoveCoroutine);

                    _lastMoveCoroutine = StartCoroutine(PlayerMoveTowards(hit.point));
                    _targetPos = hit.point;
                }

                else if (hit.collider.gameObject.layer.CompareTo(_enemyLayer) == 0)
                    print("Pressed on enemy!");

                else if (hit.collider.gameObject.layer.CompareTo(_itemLayer) == 0)
                    print("Pressed on Item!");
            }
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 targetPos)
    {
        targetPos.y = 0f;

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 movement = direction.normalized * _player.MovementSpeed * Time.deltaTime;

            _characterController.Move(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _player.RotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    #endregion

    #region Keyboard Input

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        OnInventoryPressed?.Invoke();
    }

    public void OnInventoryUIButtonPressed()
    {
        OnInventoryPressed?.Invoke();

    }

    #endregion

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_targetPos, 0.5f);
    }

    #endregion
}
