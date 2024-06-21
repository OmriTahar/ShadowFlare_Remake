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
        [SerializeField] protected bool IsActive = true;

        [Header("Debug")]
        [SerializeField] protected float DistanceFromPlayer;

        protected EnemyModel Model;
        protected Transform PlayerTransform;

        #region Initialization

        public EnemyModel InitEnemy(IUnit unit, Transform playerTransform)
        {
            PlayerTransform = playerTransform;
            name = unit.Stats.Name;

            CacheNulls();
            RegisterEvents();
            SetModel(unit);
            SetNavMeshAgent();

            return Model;
        }

        protected virtual void SetModel(IUnit unit)
        {
            Model = new EnemyModel(unit);
            View.SetModel(Model);
        }

        private void CacheNulls()
        {
            if(View == null)
                View = GetComponentInChildren<EnemyView>();
        }

        private void SetNavMeshAgent()
        {
            Agent.speed = Model.Stats.MovementSpeed;
        }

        #endregion

        #region MonoBehaviour

        protected virtual void Update()
        {
            if(!IsActive || !Agent.isActiveAndEnabled)
                return;

            DistanceFromPlayer = Vector3.Distance(Agent.transform.position, PlayerTransform.position);
        }

        private void OnDisable()
        {
            DeregisterEvents();
        }

        #endregion

        #region Game Manager Helpers

        public void SetEnemyUnitAndUnitHandler(IUnit unit)
        {
            Model.SetEnemyUnitAndUnitHandler(unit);
        }

        public Collider GetEnemyCollider()
        {
            return View.GetEnemyCollider();
        }

        #endregion

        #region Events

        private void RegisterEvents()
        {
            View.OnAttackAnimationEnded += HandleAttackAnimationEnded;
            View.OnTriggerEnterEvent += HandleTriggerEnter;
            View.OnDeath += HandleDeath;
            View.OnFinishedFadeOutAnimation += HandleDeathAnimationFinished;
        }

        private void DeregisterEvents()
        {
            View.OnAttackAnimationEnded -= HandleAttackAnimationEnded;
            View.OnTriggerEnterEvent -= HandleTriggerEnter;
            View.OnDeath -= HandleDeath;
            View.OnFinishedFadeOutAnimation -= HandleDeathAnimationFinished;
        }

        #endregion

        #region Comabt & Death

        private void HandleTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == AttackLayer)
            {
                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack, this);
            }
        }

        protected abstract void HandleAttackAnimationEnded();

        private void HandleDeath()
        {
            OnDeath?.Invoke(Model.Stats);
            Model.SetEnemyState(EnemyState.Dead);
        }

        private void HandleDeathAnimationFinished()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}


