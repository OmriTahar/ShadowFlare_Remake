using ShadowFlareRemake.Combat;
using ShadowFlareRemake.PlayerInputReader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowFlareRemake.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : LayersAndTagsReader
    {
        public event Action<Attack> OnIGotHit;
        public event Action<Collider> OnPickedLoot;
        public event Action<bool> OnPlayerAttack;
        public event Action OnStartTalkingToNpc;
        public event Action OnFinishTalkingToNpc;

        [Header("References")]
        [SerializeField] private PlayerView _view;
        [SerializeField] private Transform _frontRayOrigin;
        [SerializeField] private List<Attack> _attacks;

        [Header("Movement Settings")]
        [SerializeField] private float _stepForwardDuration = 0.2f;

        [Header("Attack Settings")]
        [SerializeField] private float _attackDistance = 2f;
        [SerializeField] private float _stepForwardDistance = 1.5f;
        [SerializeField] private float _pickUpDistance = 1.5f;

        private const int _rotationSpeed = 10;
        private const float _destinationReachedThreshold = 0.1f;
        private const float _maxMoveDuration = 3;

        private BasePlayerModel _model;
        private CharacterController _characterController;
        private IPlayerInputReader _inputReader;

        private Coroutine _lastMoveCoroutine;
        private Vector3 _lastTargetPos;
        private Ray _forwardRay;
        private bool _isLastActionWasMove = false;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            CacheNulls();
        }

        private void Update()
        {
            if(_inputReader.IsLeftMouseHeldDown)
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

        public void InitPlayer(BasePlayerModel basePlayerModel, IPlayerInputReader inputManager)
        {
            _model = basePlayerModel;
            _view.SetModel(_model);

            _inputReader = inputManager;

            RegisterEvents();
            SetStatsInAttacks();
        }

        private void RegisterLeftClickActions(InputAction.CallbackContext context)
        {
            HandleLeftClickActions(false);
        }

        private void SetStatsInAttacks()
        {
            foreach(var attack in _attacks)
            {
                attack.SetUnitStats(_model.Stats);
            }
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            _inputReader.ResigterToMouseInputAction(PlayerMouseInputType.LeftMouse, RegisterLeftClickActions);
            _inputReader.ResigterToMouseInputAction(PlayerMouseInputType.RightMouse, UseSkillAtDirection);

            _view.OnAttackAnimationEnded += SetIsAttackingToFalse;
            _view.OnDoStepForwardAnimationEvent += HandleAttackStepForward;
            _view.OnTriggerEnterEvent += HandleTriggerEnter;
        }

        private void DeregisterEvents()
        {
            _inputReader.DeresigterFromMouseInputAction(PlayerMouseInputType.LeftMouse, RegisterLeftClickActions);
            _inputReader.DeresigterFromMouseInputAction(PlayerMouseInputType.RightMouse, UseSkillAtDirection);

            _view.OnAttackAnimationEnded -= SetIsAttackingToFalse;
            _view.OnDoStepForwardAnimationEvent -= HandleAttackStepForward;
            _view.OnTriggerEnterEvent -= HandleTriggerEnter;
        }

        #endregion

        #region Meat & Potatos

        private void HandleLeftClickActions(bool isLeftMouseHeldDown)
        {
            if(!IsValidLeftMouseClick(isLeftMouseHeldDown))
                return;

            var hit = _inputReader.CurrentRaycastHit;

            if(hit.collider == null)
                return;

            IEnumerator selectedCoroutine = null;

            if((isLeftMouseHeldDown && _isLastActionWasMove) || _inputReader.IsCursorOnGround)
            {
                selectedCoroutine = MoveLogic(hit.point);
                SetIsLastActionWasMove(true);
            }
            else if(_inputReader.IsCursorOnEnemy)
            {
                var hitCollider = _inputReader.CurrentRaycastHitCollider;
                var enemyPos = hitCollider.transform.position;
                selectedCoroutine = MoveAndAttackLogic(enemyPos);
                SetIsLastActionWasMove(false);
            }
            else if(_inputReader.IsCursorOnNPC)
            {
                var hitCollider = _inputReader.CurrentRaycastHitCollider;
                var npcPos = hitCollider.transform.position;

                selectedCoroutine = MoveAndTalkLogic(npcPos);
                SetIsLastActionWasMove(false);
            }
            else if(_inputReader.IsCursorOnItem)
            {
                selectedCoroutine = MoveAndPickUpLogic(hit.point, hit.collider);
                SetIsLastActionWasMove(false);
            }

            ////// DO THIS BETTER
            if(_model.IsTalking && !_inputReader.IsCursorOnNPC)
            {
                _model.SetIsTalking(false);
                OnFinishTalkingToNpc?.Invoke();
            }

            HandleCoroutines(selectedCoroutine);
        }

        private bool IsValidLeftMouseClick(bool isLeftMouseHeldDown)
        {
            if(_model.IsAttacking || _inputReader.IsCursorOnUI || (isLeftMouseHeldDown && !_isLastActionWasMove) || _inputReader.IsHoldingLoot)
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
            var elapsedTime = 0f;
            var movementSpeed = _model.GetMovementSpeedForMoveLogic();

            _model.SetIsMoving(true);

            while(Vector3.Distance(transform.position, targetPos) > _destinationReachedThreshold && elapsedTime < _maxMoveDuration)
            {
                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _model.SetIsMoving(false);
        }

        private IEnumerator MoveAndAttackLogic(Vector3 targetPos)
        {
            targetPos.y = 0f;
            var elapsedTime = 0f;
            var movementSpeed = _model.GetMovementSpeedForMoveLogic();

            if(Vector3.Distance(transform.position, targetPos) <= _attackDistance)
            {
                var targetDirection = new Vector3(targetPos.x, 0, targetPos.z);
                transform.LookAt(targetDirection);
                Attack(false);
                yield break;
            }

            _model.SetIsMoving(true);

            while(Vector3.Distance(transform.position, targetPos) > _attackDistance && elapsedTime < _maxMoveDuration)
            {
                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }

            _model.SetIsMoving(false);

            Attack(false);
        }

        private IEnumerator MoveAndTalkLogic(Vector3 targetPos)
        {
            targetPos.y = 0f;
            var elapsedTime = 0f;
            var movementSpeed = _model.GetMovementSpeedForMoveLogic();

            if(Vector3.Distance(transform.position, targetPos) <= _attackDistance)
            {
                var targetDirection = new Vector3(targetPos.x, 0, targetPos.z);
                transform.LookAt(targetDirection);

                _model.SetIsTalking(true);
                OnStartTalkingToNpc?.Invoke();
                yield break;
            }

            _model.SetIsMoving(true);

            while(Vector3.Distance(transform.position, targetPos) > _attackDistance && elapsedTime < _maxMoveDuration)
            {
                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }

            _model.SetIsMoving(false);
            _model.SetIsTalking(true);
            OnStartTalkingToNpc?.Invoke();
        }

        private void UseSkillAtDirection(InputAction.CallbackContext context)
        {
            if(_model.IsAttacking || _inputReader.IsCursorOnUI)
                return;

            if(_lastMoveCoroutine != null)
            {
                StopCoroutine(_lastMoveCoroutine);
            }

            var raycastHit = _inputReader.CurrentRaycastHit;
            _lastTargetPos = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
            transform.LookAt(_lastTargetPos);

            Attack(true);
        }

        private void Attack(bool isUsingSkill)
        {
            OnPlayerAttack?.Invoke(isUsingSkill);
        }

        public void HandleAttackStepForward()
        {
            if(_lastMoveCoroutine != null)
                StopCoroutine(_lastMoveCoroutine);

            _lastMoveCoroutine = StartCoroutine(AttackStepForwardLogic(_stepForwardDuration));
        }

        private IEnumerator AttackStepForwardLogic(float timeToComplete)
        {
            float timer = 0;
            var movementSpeed = _model.GetMovementSpeedForMoveLogic();
            _forwardRay = new Ray(_frontRayOrigin.position, transform.forward);

            while(timer < timeToComplete)
            {
                if(Physics.Raycast(_forwardRay, out RaycastHit hit, _stepForwardDistance))
                {
                    if(hit.collider.gameObject.layer == EnemyLayer)
                        break;
                }

                Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);

                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void SetIsAttackingToFalse()
        {
            _model.SetIsAttackingToFalse();
        }

        private IEnumerator MoveAndPickUpLogic(Vector3 targetPos, Collider lootCollider)
        {
            targetPos.y = 0f;
            var elapsedTime = 0f;
            var movementSpeed = _model.GetMovementSpeedForMoveLogic();

            if(Vector3.Distance(transform.position, targetPos) <= _pickUpDistance)
            {
                var targetDirection = new Vector3(targetPos.x, 0, targetPos.z);
                transform.LookAt(targetDirection);
                OnPickedLoot?.Invoke(lootCollider);
                yield break;
            }

            _model.SetIsMoving(true);

            while(Vector3.Distance(transform.position, targetPos) > _pickUpDistance && elapsedTime < _maxMoveDuration)
            {
                Vector3 direction = targetPos - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 movement = direction.normalized * movementSpeed * Time.deltaTime;
                _characterController.Move(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), _rotationSpeed * Time.deltaTime);
                yield return null;
            }

            _model.SetIsMoving(false);

            OnPickedLoot?.Invoke(lootCollider);
        }

        public void SetIsLastActionWasMove(bool isLastActionWasMove)
        {
            _isLastActionWasMove = isLastActionWasMove;
        }

        #endregion 

        #region Got Hit

        private void HandleTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == EnemyLayer && other.gameObject.tag == AttackTag)
            {
                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack);
            }
        }

        #endregion

        #region Gizmos

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_forwardRay.origin, _forwardRay.direction * _attackDistance);
        }

        #endregion
    }
}


