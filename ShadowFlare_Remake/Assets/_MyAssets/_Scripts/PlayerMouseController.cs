using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMouseController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _movementSpeed = 5;
    [SerializeField] private int _rotationSpeed = 5;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _enemyLayers;
    [SerializeField] private LayerMask _itemLayers;

    [Header("Cursors")]
    public Texture2D MoveCursor;
    public Texture2D AttackCursor;
    public Texture2D ItemCursor;
    public Texture2D MenusCursor;

    [Header("References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private InputAction _leftMouseClickAction;

    private CharacterController _characterController;
    private Ray _currentMouseRay;
    private Vector3 _targetPos;
    private Coroutine _lastMoveCoroutine;


    private void Awake()
    {
        ChangeCursor(MoveCursor);
        Cursor.lockState = CursorLockMode.Confined;

        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _leftMouseClickAction.Enable();
        _leftMouseClickAction.performed += Move;
    }

    private void OnDisable()
    {
        _leftMouseClickAction.performed -= Move;
        _leftMouseClickAction.Disable();
    }

    private void Update()
    {
        CursorController();
    }

    private void CursorController()
    {
        _currentMouseRay = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(_currentMouseRay, out RaycastHit hit))
        {
            if (hit.collider)
            {
                if (((1 << hit.collider.gameObject.layer) & _groundLayers) != 0)
                    ChangeCursor(MoveCursor);

                else if (((1 << hit.collider.gameObject.layer) & _enemyLayers) != 0)
                    ChangeCursor(AttackCursor);

                else if (((1 << hit.collider.gameObject.layer) & _itemLayers) != 0)
                    ChangeCursor(ItemCursor);

                else
                    ChangeCursor(MenusCursor);
            }
            else
                ChangeCursor(MenusCursor);
        }
    }

    private void ChangeCursor(Texture2D newCursor)
    {
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.Auto);
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(_currentMouseRay, out RaycastHit hit) && hit.collider)
        {
            if (((1 << hit.collider.gameObject.layer) & _groundLayers) != 0)
            {
                if (_lastMoveCoroutine != null)
                    StopCoroutine(_lastMoveCoroutine);

                _lastMoveCoroutine = StartCoroutine(PlayerMoveTowards(hit.point));
                _targetPos = hit.point;
            }
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 targetPos)
    {
        float playerDistanceToFloor = transform.position.y - _targetPos.y;
        targetPos.y += playerDistanceToFloor;

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, targetPos, _movementSpeed * Time.deltaTime);
            Vector3 direction = destination - transform.position;
            Vector3 movementSpeed = direction.normalized * _movementSpeed * Time.deltaTime;

            _characterController.Move(movementSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_targetPos, 1);
    }

}
