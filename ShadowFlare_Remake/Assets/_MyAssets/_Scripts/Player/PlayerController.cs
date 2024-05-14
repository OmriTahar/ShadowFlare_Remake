using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using ShadowFlareRemake.Combat;
using System;
using System.Threading.Tasks;
using ShadowFlareRemake.PlayerInput;

namespace ShadowFlareRemake.Player {

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Controller {

        public event Action<Attack, IUnitStats> OnIGotHit;

        [Header("References")]
        [SerializeField] private PlayerView _view;
        [SerializeField] private Attack _meleeAttack;

        [Header("Movement Settings")]
        [SerializeField] private float _stepForwardDuration = 0.2f;

        [Header("Attack Settings")]
        [SerializeField] private float _attackDistance = 2f;

        private PlayerModel _model;
        private CharacterController _characterController;
        private InputManager _inputManager;
        private Coroutine _lastMoveCoroutine;

        private const int _rotationSpeed = 10;
        private bool _isAttacking = false;

        #region Unity Callbacks

        protected override void Awake() {

            base.Awake();
            CacheNulls();
        }

        private void Update() {

            if(_inputManager.IsLeftMouseIsHeldDown) {
                HandleLeftClickActions();
            }
        }

        private void FixedUpdate() {

            if(transform.position.y > 0.1) {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            if(transform.rotation.x > 0.1) {
                transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
            }
        }

        private void OnDestroy() {

            DeregisterEvents();
        }

        #endregion 

        #region Initialization

        public async Task InitPlayer(IUnit unit) {

            _model = new PlayerModel(unit);
            _view.SetModel(_model);

            _inputManager = InputManager.Instance;
            await _inputManager.WaitForInitFinish();
            RegisterEvents();

            _meleeAttack.SetUnitStats(unit.Stats);
        }

        private void CacheNulls() {

            if(_characterController == null) {
                _characterController = GetComponent<CharacterController>();
            }
            if(_view == null) {
                _view = GetComponentInChildren<PlayerView>();
            }
        }

        private void RegisterLeftClickActions(InputAction.CallbackContext context) {

            HandleLeftClickActions();
        }

        private void RegisterEvents() {

            _inputManager.LeftMouseClickAction.performed += RegisterLeftClickActions;
            _inputManager.RightMouseClickAction.performed += AttackAtDirection;

            _view.OnAttackAnimationEnded += ResetAttackCooldown;
            _view.OnDoStepForwardAnimationEvent += HandleStepForward;
            _view.OnTriggerEnterEvent += HandleTriggerEnter;
        }

        private void DeregisterEvents() {

            _inputManager.LeftMouseClickAction.performed -= RegisterLeftClickActions;
            _inputManager.RightMouseClickAction.performed -= AttackAtDirection;

            _view.OnAttackAnimationEnded -= ResetAttackCooldown;
            _view.OnDoStepForwardAnimationEvent -= HandleStepForward;
            _view.OnTriggerEnterEvent -= HandleTriggerEnter;
        }

        #endregion 

        #region Move & Attack

        private void HandleLeftClickActions() {

            if(_isAttacking || _inputManager.IsCursorOnUI) {
                return;
            }

            var raycastHit = _inputManager.CurrentRaycastHit;

            if(raycastHit.collider) {

                if(_inputManager.IsCursorOnGround) {
                    HandleMove(raycastHit);

                } else if(_inputManager.IsCursorOnEnemy) {
                    var enemyPos = raycastHit.collider.transform.position;
                    HandleMoveAndAttack(enemyPos);

                } else if(_inputManager.IsCursorOnItem) {
                    print("Pressed on Item - Collecting!");
                }
            }
        }

        private void HandleMove(RaycastHit raycastHit) {

            if(_lastMoveCoroutine != null)
                StopCoroutine(_lastMoveCoroutine);

            _lastMoveCoroutine = StartCoroutine(MoveLogic(raycastHit.point));
        }

        private void HandleMoveAndAttack(Vector3 enemyPos) {

            if(_lastMoveCoroutine != null)
                StopCoroutine(_lastMoveCoroutine);

            _lastMoveCoroutine = StartCoroutine(MoveAndAttackLogic(enemyPos));
        }

        public void HandleStepForward() {

            if(_lastMoveCoroutine != null)
                StopCoroutine(_lastMoveCoroutine);

            _lastMoveCoroutine = StartCoroutine(StepForwardLogic(_stepForwardDuration));
        }

        private IEnumerator MoveLogic(Vector3 targetPos) {

            targetPos.y = 0f;
            var movementSpeed = _model.Stats.MovementSpeed; 

            while(Vector3.Distance(transform.position, targetPos) > 0.1f) {

                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator MoveAndAttackLogic(Vector3 targetPos) {

            targetPos.y = 0f;
            var movementSpeed = _model.Stats.MovementSpeed;

            if(Vector3.Distance(transform.position, targetPos) <= _attackDistance) {

                var targetDirection = new Vector3(targetPos.x, 0, targetPos.z);
                transform.LookAt(targetDirection);
                Attack(PlayerModel.AttackType.Single);
                yield break;
            }

            while(Vector3.Distance(transform.position, targetPos) > _attackDistance) {

                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }

            Attack(PlayerModel.AttackType.Single);
        }

        private IEnumerator StepForwardLogic(float timeToComplete) {

            float timer = 0;
            var movementSpeed = _model.Stats.MovementSpeed;

            while(timer < timeToComplete) {

                Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void AttackAtDirection(InputAction.CallbackContext context) {

            if(_isAttacking || _inputManager.IsCursorOnUI) {
                return;
            }

            if(_lastMoveCoroutine != null) {
                StopCoroutine(_lastMoveCoroutine);
            }

            var raycastHit = _inputManager.CurrentRaycastHit;

            if(raycastHit.collider) {

                var targetDirection = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
                transform.LookAt(targetDirection);
                Attack(PlayerModel.AttackType.ThreeStrikes);
            }
        }

        private void Attack(PlayerModel.AttackType attackType) {

            _model.SetAttackState(true, attackType);
            _isAttacking = true;
        }

        private void ResetAttackCooldown() {

            _model.SetAttackState(false);
            _isAttacking = false;
        }

        #endregion Move & Attack

        #region Got Hit

        private void HandleTriggerEnter(Collider other) {

            if(other.gameObject.layer == AttackLayer) {

                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack, _model.Stats);
            }
        }

        #endregion
    }
}


