using UnityEngine;

namespace ShadowFlareRemake {
    public abstract class LayersAndTagsReader : MonoBehaviour {

        protected int PlayerLayer;
        protected int GroundLayer;
        protected int EnemyLayer;
        protected int ItemLayer;
        protected int UILayer;

        protected string AttackTag;

        protected virtual void Awake() {

            InitLayers();
            InitTags();
        }

        private void InitLayers() {

            PlayerLayer = LayerMask.NameToLayer("Player");
            GroundLayer = LayerMask.NameToLayer("Ground");
            EnemyLayer = LayerMask.NameToLayer("Enemy");
            ItemLayer = LayerMask.NameToLayer("Item");
            UILayer = LayerMask.NameToLayer("UI");
        }

        private void InitTags()
        {
            AttackTag = "Attack";
        }
    }
}
