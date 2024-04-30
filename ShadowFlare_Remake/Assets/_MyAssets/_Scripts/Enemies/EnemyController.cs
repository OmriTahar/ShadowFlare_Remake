using ShadowFlareRemake.Combat;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace ShadowFlareRemake.Enemies {

    public abstract class EnemyController : Controller {

        public event Action<Attack, EnemyController> OnIGotHit;
        public event Action<IEnemyUnitStats> OnIGotKilled;

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

        public void InitEnemy(IUnit unit, Transform playerTransform) {

            PlayerTransform = playerTransform;

            CacheNulls();
            RegisterEvents();
            SetModel(unit);
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
      
        protected virtual void SetModel(IUnit unit) {

            Model = new EnemyModel(unit);
            View.SetModel(Model);
        }

        public void SetEnemyUnitAndUnitHandler(IUnit unit) {

            Model.SetEnemyUnitAndUnitHandler(unit);
        }


        protected abstract void Attack();

        private void HandleTriggerEnter(Collider other) {

            if(other.gameObject.layer == AttackLayer) {

                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack, this);
            }
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
        }

        private void RegisterEvents() {

            View.OnCurserEntered += SelectEnemy;
            View.OnCurserLeft += DeselectEnemy;
            View.OnTriggerEnterEvent += HandleTriggerEnter;
            View.OnFinishedDeathAnimation += HandleDeath;
            View.OnAttackAnimationEnded += ResetAttackCooldown;
        }

        private void DeregisterEvents() {

            View.OnCurserEntered -= SelectEnemy;
            View.OnCurserLeft -= DeselectEnemy;
            View.OnTriggerEnterEvent -= HandleTriggerEnter;
            View.OnFinishedDeathAnimation -= HandleDeath;
            View.OnAttackAnimationEnded -= ResetAttackCooldown;
        }

        public void TakeDamage(int damage) {
            throw new NotImplementedException();
        }
    }
}


