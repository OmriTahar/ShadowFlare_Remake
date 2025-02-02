using UnityEngine;

namespace ShadowFlareRemake {
    public abstract class LayersAndTagsReader : MonoBehaviour {

        protected int PlayerLayer;
        protected int GroundLayer;
        protected int EnemyLayer;
        protected int NpcLayer;
        protected int InteractableLayer;
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
            NpcLayer = LayerMask.NameToLayer("NPC");
            InteractableLayer = LayerMask.NameToLayer("Interactable");
            ItemLayer = LayerMask.NameToLayer("Item");
            UILayer = LayerMask.NameToLayer("UI");
        }

        private void InitTags()
        {
            AttackTag = "Attack";
        }
    }
}

