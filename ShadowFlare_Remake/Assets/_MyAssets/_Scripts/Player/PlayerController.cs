using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using ShadowFlareRemake.Combat;

namespace ShadowFlareRemake.Player {

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Controller {

        [Header("References")]
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PlayerUnit _unit;

        [Header("General Settings")]
        [SerializeField] private bool _allowPlayerInput = true;

        [Header("Movement Settings")]
        [SerializeField] private int _rotationSpeed = 10;
        [SerializeField] private float _stepForwardDuration = 0.2f;

        [Header("Attack Settings")]
        [SerializeField] private float _attackDistance = 2f;

        private PlayerModel _playerModel;
        private CharacterController _characterController;
        private Coroutine _lastMoveCoroutine;

        #region Unity Callbacks
        protected virtual void Awake() {

            base.Init();
            CacheNulls();

            _playerModel = new PlayerModel(_unit);
            _playerView.SetModel(_playerModel);
        }

        private async void OnEnable() {

            await AwaitPlayerInputInit();
            RegisterEvents();
        }

        private void OnDisable() {
            DeregisterEvents();
        }

        private void Update() {

            if(PlayerInput.Instance.IsLeftMouseIsHeldDown) {
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
        #endregion Unity Callbacks

        #region Initialization
        private async Task AwaitPlayerInputInit() {

            while(PlayerInput.Instance == null || PlayerInput.Instance.enabled == false) {
                await Task.Delay(100);
            }
        }

        private void CacheNulls() {

            if(_characterController == null) {
                _characterController = GetComponent<CharacterController>();
            }
            if(_playerView == null) {
                _playerView = GetComponentInChildren<PlayerView>();
            }
        }

        private void RegisterLeftClickActions(InputAction.CallbackContext context) {
            HandleLeftClickActions();
        }

        private void RegisterEvents() {

            PlayerInput.Instance.LeftMouseClickAction.performed += RegisterLeftClickActions;
            PlayerInput.Instance.RightMouseClickAction.performed += AttackAtDirection;

            _playerView.OnAttackAnimationEnded += ResetAttackCooldown;
            _playerView.OnDoStepForwardAnimationEvent += HandleStepForward;
            _playerView.OnTriggerEnterEvent += HandleTriggerEnter;
        }

        private void DeregisterEvents() {

            PlayerInput.Instance.LeftMouseClickAction.performed -= RegisterLeftClickActions;
            PlayerInput.Instance.RightMouseClickAction.performed -= AttackAtDirection;

            _playerView.OnAttackAnimationEnded -= ResetAttackCooldown;
            _playerView.OnDoStepForwardAnimationEvent -= HandleStepForward;
            _playerView.OnTriggerEnterEvent -= HandleTriggerEnter;
        }
        #endregion Initialization

        #region Move & Attack
        private void HandleLeftClickActions() {

            if(_allowPlayerInput == false || PlayerInput.Instance.IsCursorOnUI) {
                return;
            }

            var raycastHit = PlayerInput.Instance.CurrentRaycastHit;

            if(raycastHit.collider) {

                var raycastLayer = raycastHit.collider.gameObject.layer;
                var IsGroundLayer = raycastLayer.CompareTo(GroundLayer) == 0;
                var IsEnemyLayer = raycastLayer.CompareTo(EnemyLayer) == 0;
                var IsItemLayer = raycastLayer.CompareTo(ItemLayer) == 0;

                if(IsGroundLayer) {
                    HandleMove(raycastHit);

                } else if(IsEnemyLayer) {
                    var enemyPos = raycastHit.collider.transform.position;
                    HandleMoveAndAttack(enemyPos);

                } else if(IsItemLayer) {
                    print("Pressed on Item - Collecting!");

                } else {
                    print("You are clicking on thin air sugerpups");
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
            var movementSpeed = _unit.Stats.WalkingSpeed;

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
            var movementSpeed = _unit.Stats.WalkingSpeed;

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
            var movementSpeed = _unit.Stats.WalkingSpeed;

            while(timer < timeToComplete) {

                Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void AttackAtDirection(InputAction.CallbackContext context) {

            if(_allowPlayerInput == false || PlayerInput.Instance.IsCursorOnUI) {
                return;
            }

            if(_lastMoveCoroutine != null) {
                StopCoroutine(_lastMoveCoroutine);
            }

            var raycastHit = PlayerInput.Instance.CurrentRaycastHit;

            if(raycastHit.collider) {

                var targetDirection = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
                transform.LookAt(targetDirection);
                Attack(PlayerModel.AttackType.ThreeStrikes);
            }
        }

        private void Attack(PlayerModel.AttackType attackType) {

            _playerModel.SetAttackState(true, attackType);
            _allowPlayerInput = false;
        }

        private void ResetAttackCooldown() {

            _playerModel.SetAttackState(false);
            _allowPlayerInput = true;
        }
        #endregion Move & Attack

        #region TakeDamage
        private void HandleTriggerEnter(Collider other) {

            if(other.gameObject.layer == AttackLayer) {

                var attack = other.GetComponent<Attack>();
                CombatUtils.HandleTakeDamage(attack, _unit);
            }
        }

        private void TakeDamage(int damage) {

            //_playerModel.TakeDamage(damage);
            //if(!_playerModel.CanTakeDamage) {
            //    StartCoroutine(TakeDamageCooldown(_takeDamageCooldownDuration));
            //}
        }

        //private IEnumerator TakeDamageCooldown(float cooldown) {
        //    yield return new WaitForSeconds(cooldown);
        //    _playerModel.SetCanTakeDamage(true);
        //}
        #endregion TakeDamage
    }
}


