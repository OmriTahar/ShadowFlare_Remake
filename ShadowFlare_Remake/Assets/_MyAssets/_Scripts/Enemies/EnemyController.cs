using UnityEngine;
using UnityEngine.AI;
using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Rewards;
using System;

namespace ShadowFlareRemake.Enemies {

    public abstract class EnemyController : Controller {

        public event Action<Attack, EnemyController> OnIGotHit;

        [field: SerializeField] public IUnit Unit { get; private set; }

        [Header("Base References")]
        [SerializeField] protected EnemyView View;
        [SerializeField] protected NavMeshAgent Agent;

        [Header("Base Settings")]
        [SerializeField] protected bool ChasePlayer = false;
        [SerializeField] protected float AttackDistance = 0.5f;

        [Header("Base Status")]
        [SerializeField] protected bool IsActive = true;
        [SerializeField] protected float DistanceFromPlayer;

        protected EnemyModel Model;
        protected Transform PlayerTransform;

        protected bool IsAllowedToAttack = true;

        public override void Init() {

            base.Init();
            CacheNulls();
            RegisterEvents();
            SetModel();
        }

        public void InitEnemy(Transform playerTransform) {

            PlayerTransform = playerTransform;
        }

        protected virtual void Update() {

            if(!IsActive) {
                return;
            }

            DistanceFromPlayer = Vector3.Distance(Agent.transform.position, PlayerTransform.position);

            if(ChasePlayer) {
                Agent.SetDestination(PlayerTransform.position);
            }
        }

        private void OnDestroy() {
            DeregisterEvents();
        }

        public void SetUnit(IUnit unit) {

            Unit = unit;
            Model.InvokeChanged();
        }

        protected virtual void SetModel() {

            Model = new EnemyModel(Unit);
            View.SetModel(Model);
        }

        protected abstract void Attack();

        private void HandleTriggerEnter(Collider other) {

            if(other.gameObject.layer == AttackLayer) {

                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack, this);
            }
        }

        private void InformRewardsManager() {
            //RewardsManager.Instance.HandleEnemyKilledRewards(Unit.UnitStats);
        }

        private void HandleDeath() {
            Destroy(gameObject);
        }

        private void SelectEnemy() {
            Model.UpdateIsEnemyHighlighted(true);
        }

        private void DeselectEnemy() {
            Model.UpdateIsEnemyHighlighted(false);
        }

        private void ResetAttackCooldown() {
            Model.UpdateAttackState(false, Enums.AttackMethod.None);
            IsAllowedToAttack = true;
        }

        private void CacheNulls() {

            if(View == null) {
                View = GetComponentInChildren<EnemyView>();
            }
            if(PlayerTransform == null) {
                PlayerTransform = GameManager.Instance.PlayerTransform;
            }
            if(Unit == null) {
                Unit = GetComponent<Unit>();
            }
        }
     
        private void RegisterEvents() {

            View.OnCurserEntered += SelectEnemy;
            View.OnCurserLeft += DeselectEnemy;
            View.OnTriggerEnterEvent += HandleTriggerEnter;
            View.OnEnemyKilled += InformRewardsManager;
            View.OnFinishedDeathAnimation += HandleDeath;
            View.OnAttackAnimationEnded += ResetAttackCooldown;
        }

        private void DeregisterEvents() {

            View.OnCurserEntered -= SelectEnemy;
            View.OnCurserLeft -= DeselectEnemy;
            View.OnTriggerEnterEvent -= HandleTriggerEnter;
            View.OnEnemyKilled -= InformRewardsManager;
            View.OnFinishedDeathAnimation -= HandleDeath;
            View.OnAttackAnimationEnded -= ResetAttackCooldown;
        }
    }
}


