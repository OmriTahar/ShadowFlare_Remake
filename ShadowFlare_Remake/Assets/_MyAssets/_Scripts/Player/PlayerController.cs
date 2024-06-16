using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enums;
using ShadowFlareRemake.PlayerInput;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Controller
    {
        public event Action<Attack, IUnitStats> OnIGotHit;
        public event Action<Collider> OnPickedLoot;

        [Header("References")]
        [SerializeField] private PlayerView _view;
        [SerializeField] private Attack _meleeAttack;

        [Header("Movement Settings")]
        [SerializeField] private float _stepForwardDuration = 0.2f;

        [Header("Attack Settings")]
        [SerializeField] private float _attackDistance = 2f;
        [SerializeField] private float _pickUpDistance = 1.5f;

        private PlayerModel _model;
        private CharacterController _characterController;
        private IInputManager _inputManager;
        private Coroutine _lastMoveCoroutine;

        private const int _rotationSpeed = 10;

        private bool _isAttacking = false;
        private bool _isLastActionWasMove = false;

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            CacheNulls();
        }

        private void Update()
        {
            if(_inputManager.IsLeftMouseHeldDown)
            {
                HandleLeftClickActions(true);
            }
        }

        private void FixedUpdate()
        {
            if(transform.position.y > 0.1)
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            if(transform.rotation.x > 0.1)
                transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
        }

        private void OnDestroy()
        {
            DeregisterEvents();
        }

        #endregion

        #region Initialization

        private void CacheNulls()
        {
            if(_characterController == null)
                _characterController = GetComponent<CharacterController>();

            if(_view == null)
                _view = GetComponentInChildren<PlayerView>();
        }

        public void InitPlayer(IUnit unit, IInputManager inputManager)
        {
            _model = new PlayerModel(unit);
            _view.SetModel(_model);

            _inputManager = inputManager;

            RegisterEvents();

            _meleeAttack.SetUnitStats(unit.Stats);
        }

        private void RegisterLeftClickActions(InputAction.CallbackContext context)
        {
            HandleLeftClickActions(false);
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _inputManager.ResigterToInputAction(PlayerInputType.LeftMouse, RegisterLeftClickActions);
            _inputManager.ResigterToInputAction(PlayerInputType.RightMouse, AttackAtDirection);

            _view.OnAttackAnimationEnded += ResetAttackCooldown;
            _view.OnDoStepForwardAnimationEvent += HandleAttackStepForward;
            _view.OnTriggerEnterEvent += HandleTriggerEnter;
        }

        private void DeregisterEvents()
        {
            _inputManager.DeresigterFromInputAction(PlayerInputType.LeftMouse, RegisterLeftClickActions);
            _inputManager.DeresigterFromInputAction(PlayerInputType.RightMouse, AttackAtDirection);

            _view.OnAttackAnimationEnded -= ResetAttackCooldown;
            _view.OnDoStepForwardAnimationEvent -= HandleAttackStepForward;
            _view.OnTriggerEnterEvent -= HandleTriggerEnter;
        }

        #endregion

        #region Meat & Potatos

        private void HandleLeftClickActions(bool isLeftMouseHeldDown)
        {
            if(!IsValidLeftMouseClick(isLeftMouseHeldDown))
                return;

            var hit = _inputManager.CurrentRaycastHit;

            if(hit.collider == null)
                return;

            IEnumerator selectedCoroutine = null;

            if((isLeftMouseHeldDown && _isLastActionWasMove) || _inputManager.IsCursorOnGround)
            {
                selectedCoroutine = MoveLogic(hit.point);
                _isLastActionWasMove = true;
            }
            else if(_inputManager.IsCursorOnEnemy)
            {
                var hitCollider = _inputManager.CurrentRaycastHitCollider;
                var enemyPos = hitCollider.transform.position;
                selectedCoroutine = MoveAndAttackLogic(enemyPos);
                _isLastActionWasMove = false;
            }
            else if(_inputManager.IsCursorOnItem)
            {
                selectedCoroutine = MoveAndPickUpLogic(hit.point, hit.collider);
                _isLastActionWasMove = false;
            }

            HandleCoroutines(selectedCoroutine);
        }

        private bool IsValidLeftMouseClick(bool isLeftMouseHeldDown)
        {
            if(_isAttacking || _inputManager.IsCursorOnUI || (isLeftMouseHeldDown && !_isLastActionWasMove) || _inputManager.IsHoldingLoot)
            {
                return false;
            }

            return true;
        }

        private void HandleCoroutines(IEnumerator coroutine)
        {
            if(coroutine == null)
                return;

            if(_lastMoveCoroutine != null)
                StopCoroutine(_lastMoveCoroutine);

            _lastMoveCoroutine = StartCoroutine(coroutine);
        }

        private IEnumerator MoveLogic(Vector3 targetPos)
        {
            targetPos.y = 0f;
            var movementSpeed = _model.Stats.MovementSpeed;

            while(Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator MoveAndAttackLogic(Vector3 targetPos)
        {
            targetPos.y = 0f;
            var movementSpeed = _model.Stats.MovementSpeed;

            if(Vector3.Distance(transform.position, targetPos) <= _attackDistance)
            {
                var targetDirection = new Vector3(targetPos.x, 0, targetPos.z);
                transform.LookAt(targetDirection);
                Attack(PlayerModel.AttackType.Single);
                yield break;
            }

            while(Vector3.Distance(transform.position, targetPos) > _attackDistance)
            {
                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }

            Attack(PlayerModel.AttackType.Single);
        }

        private void AttackAtDirection(InputAction.CallbackContext context)
        {
            if(_isAttacking || _inputManager.IsCursorOnUI)
                return;

            if(_lastMoveCoroutine != null)
            {
                StopCoroutine(_lastMoveCoroutine);
            }

            var raycastHit = _inputManager.CurrentRaycastHit;
            var targetDirection = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
            transform.LookAt(targetDirection);

            Attack(PlayerModel.AttackType.ThreeStrikes);
        }

        private void Attack(PlayerModel.AttackType attackType)
        {
            _model.SetAttackState(true, attackType);
            _isAttacking = true;
        }

        public void HandleAttackStepForward() // Invoked from an animation event
        {
            if(_lastMoveCoroutine != null)
                StopCoroutine(_lastMoveCoroutine);

            _lastMoveCoroutine = StartCoroutine(AttackStepForwardLogic(_stepForwardDuration));
        }

        private IEnumerator AttackStepForwardLogic(float timeToComplete)
        {
            float timer = 0;
            var movementSpeed = _model.Stats.MovementSpeed;

            while(timer < timeToComplete)
            {
                Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void ResetAttackCooldown()
        {
            _model.SetAttackState(false);
            _isAttacking = false;
        }

        private IEnumerator MoveAndPickUpLogic(Vector3 targetPos, Collider lootCollider)
        {
            targetPos.y = 0f;
            var movementSpeed = _model.Stats.MovementSpeed;

            if(Vector3.Distance(transform.position, targetPos) <= _pickUpDistance)
            {
                var targetDirection = new Vector3(targetPos.x, 0, targetPos.z);
                transform.LookAt(targetDirection);
                OnPickedLoot?.Invoke(lootCollider);
                yield break;
            }

            while(Vector3.Distance(transform.position, targetPos) > _attackDistance)
            {
                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }

            OnPickedLoot?.Invoke(lootCollider);
        }

        #endregion 

        #region Got Hit

        private void HandleTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == AttackLayer)
            {
                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack, _model.Stats);
            }
        }

        #endregion
    }
}


