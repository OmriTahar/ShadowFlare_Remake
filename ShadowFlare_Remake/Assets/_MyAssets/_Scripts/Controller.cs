using UnityEngine;
using ShadowFlareRemake.Events;

namespace ShadowFlareRemake {
    public abstract class Controller : MonoBehaviour {

        protected int PlayerLayer;
        protected int GroundLayer;
        protected int EnemyLayer;
        protected int ItemLayer;
        protected int AttackLayer;
        protected int UILayer;

        private AcceptBlock _acceptBlock;

        protected virtual void Awake() {

            InitLayers();
            _acceptBlock = new AcceptBlock();
        }

        private void OnDestroy() {

            _acceptBlock.Dispose();
        }

        protected void Accept<T>(System.Action<T> registrant) where T : IGameEvent {

            _acceptBlock.AcceptEvent(registrant);
        }

        private void InitLayers() {

            PlayerLayer = LayerMask.NameToLayer("Player");
            GroundLayer = LayerMask.NameToLayer("Ground");
            EnemyLayer = LayerMask.NameToLayer("Enemy");
            ItemLayer = LayerMask.NameToLayer("Item");
            AttackLayer = LayerMask.NameToLayer("Attack");
            UILayer = LayerMask.NameToLayer("UI");
        }
    }
}

