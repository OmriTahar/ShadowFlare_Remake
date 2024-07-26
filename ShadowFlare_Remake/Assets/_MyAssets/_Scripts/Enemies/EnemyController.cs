using ShadowFlareRemake.Combat;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace ShadowFlareRemake.Enemies
{
    public abstract class EnemyController : LayersAndTagsReader
    {
        public event Action<Attack, EnemyController> OnIGotHit;
        public event Action<IEnemyUnitStats, Vector3> OnDeath;

        [Header("Base References")]
        [SerializeField] protected EnemyView View;
        [SerializeField] protected NavMeshAgent Agent;
        [SerializeField] protected Attack MeleeAttack;

        [Header("Debug")]
        [SerializeField] protected bool IsActive = true;
        [SerializeField] protected float DistanceFromPlayer;

        protected BaseEnemyModel Model;
        protected Transform PlayerTransform;

        #region Initialization

        public void InitEnemyAndGetItsCollider(BaseEnemyModel baseEnemyModel, Transform playerTransform, bool isActive)
        {
            Model = baseEnemyModel;
            PlayerTransform = playerTransform;

            name = Model.Stats.Name;
            IsActive = isActive;
            MeleeAttack.SetUnitStats(Model.Stats);

            CacheNulls();
            RegisterEvents();

            View.SetModel(Model);

            SetNavMeshAgent();
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
            DistanceFromPlayer = Vector3.Distance(Agent.transform.position, PlayerTransform.position);
        }

        private void OnDisable()
        {
            DeregisterEvents();
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
            if(other.gameObject.layer == PlayerLayer && other.gameObject.tag == AttackTag)
            {
                var attack = other.GetComponent<Attack>();
                OnIGotHit?.Invoke(attack, this);
            }
        }

        protected abstract void HandleAttackAnimationEnded();

        private void HandleDeath()
        {
            OnDeath?.Invoke(Model.Stats, transform.position);
            Model.SetEnemyState(EnemyState.Dead);
        }

        private void HandleDeathAnimationFinished()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}


