using ShadowFlareRemake.Combat;
using ShadowFlareRemake.Enums;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace ShadowFlareRemake.Enemies
{
    public abstract class EnemyController : Controller
    {
        public event Action<Attack, EnemyController> OnIGotHit;
        public event Action<IEnemyUnitStats> OnDeath;

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

        public EnemyModel InitEnemy(IUnit unit, Transform playerTransform)
        {
            PlayerTransform = playerTransform;
            name = unit.Stats.Name;

            CacheNulls();
            RegisterEvents();
            SetModel(unit);

            return Model;
        }

        protected virtual void Update()
        {
            if(!IsActive || !Agent.isActiveAndEnabled)
                return;

            DistanceFromPlayer = Vector3.Distance(Agent.transform.position, PlayerTransform.position);
        }

        private void OnDestroy()
        {
            DeregisterEvents();
        }

        protected virtual void SetModel(IUnit unit)
        {
            Model = new EnemyModel(unit);
            View.SetModel(Model);
        }

        public void SetEnemyUnitAndUnitHandler(IUnit unit)
        {
            Model.SetEnemyUnitAndUnitHandler(unit);
        }

        public Collider GetEnemyCollider()
        {
            return View.GetEnemyCollider();
        }

        protected abstract void Attack();

        private void HandleTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == AttackLayer)
            {
                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack, this);
            }
        }

        private void HandleDeath()
        {
            OnDeath?.Invoke(Model.Stats);
            Model.SetEnemyState(EnemyState.Dead);
        }

        private void HandleDeathAnimationFinished()
        {
            Destroy(gameObject);
        }

        private void ResetAttackCooldown()
        {
            Model.UpdateAttackState(false, Enums.AttackMethod.None);
            IsAllowedToAttack = true;
        }

        private void CacheNulls()
        {
            if(View == null)
            {
                View = GetComponentInChildren<EnemyView>();
            }
        }

        private void RegisterEvents()
        {
            View.OnAttackAnimationEnded += ResetAttackCooldown;
            View.OnTriggerEnterEvent += HandleTriggerEnter;
            View.OnDeath += HandleDeath;
            View.OnFinishedFadeOutAnimation += HandleDeathAnimationFinished;
        }

        private void DeregisterEvents()
        {
            View.OnAttackAnimationEnded -= ResetAttackCooldown;
            View.OnTriggerEnterEvent -= HandleTriggerEnter;
            View.OnDeath -= HandleDeath;
            View.OnFinishedFadeOutAnimation -= HandleDeathAnimationFinished;
        }
    }
}


