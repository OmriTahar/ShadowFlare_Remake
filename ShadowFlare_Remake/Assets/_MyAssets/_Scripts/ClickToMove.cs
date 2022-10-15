using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ClickToMove : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _movementSpeed = 5;
    [SerializeField] private int _rotationSpeed = 5;
    [SerializeField] private LayerMask _groundLayers;

    [Header("References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private InputAction _leftMouseClickAction;

    private CharacterController _characterController;
    private Coroutine _lastMoveCoroutine;
    private Vector3 _targetPos;


    private void Awake()
    {
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

    private void Move(InputAction.CallbackContext context)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider)
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
