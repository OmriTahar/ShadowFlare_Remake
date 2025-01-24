using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Loot;
using ShadowFlareRemake.Npc;
using UnityEngine;

namespace ShadowFlareRemake.UI.Highlightables
{
    public class HighlightableBehaviour : MonoBehaviour
    {
        public bool IsHighlightable { get; private set; } = true;
        public bool IsHighlighted { get; private set; }

        [Header("Renderers")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

        [Header("Entity")]
        [SerializeField] private GameObject _entityObject;
        [field: SerializeField] public EntityType EntityType { get; private set; }

        [Header("Settings")]
        [SerializeField] private Vector2Int _nameBubbleOffest;
        [SerializeField] private float _highlightIntensity = 0.7f;

        private const string _highlightableTag = "Highlightable";

        private NpcBehaviour _npcBehaviour;
        private EnemyView _enemyView;
        private LootView _lootView;

        private Color _color;
        private Color _highlightColor;
        private bool _useSkinnedMeshRenderer;

        #region MonoBehaviour

        private void Start()
        {
            CacheEntityBehaviour();
            InitTag();
            InitRenderer();
            InitColors();
            HandleIsHighlightedLogic();
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            DeregisterEvents();
        }

        #endregion

        #region Initialization

        private void CacheEntityBehaviour()
        {
            switch(EntityType)
            {
                case EntityType.Npc:
                    _npcBehaviour = _entityObject.GetComponent<NpcBehaviour>();
                    break;

                case EntityType.Enemy:
                    _enemyView = _entityObject.GetComponent<EnemyView>();
                    break;

                case EntityType.Loot:
                    _lootView = _entityObject.GetComponent<LootView>();
                    break;
            }
        }

        private void InitTag()
        {
            if(gameObject.tag != _highlightableTag)
            {
                gameObject.tag = _highlightableTag;
            }
        }

        private void InitRenderer()
        {
            _useSkinnedMeshRenderer = _skinnedMeshRenderer != null;
        }

        private void InitColors()
        {
            _color = _useSkinnedMeshRenderer ? _skinnedMeshRenderer.material.color : _meshRenderer.material.color;
            _highlightColor = new Color(_color.r + _highlightIntensity, _color.g + _highlightIntensity, _color.b + _highlightIntensity, 1);
        }
       
        #endregion

        #region Events

        private void RegisterEvents()
        {
            if(_enemyView == null)
                return;

            _enemyView.OnDeath += DisableHighlightable;
        }

        private void DeregisterEvents()
        {
            if(_enemyView == null)
                return;

            _enemyView.OnDeath -= DisableHighlightable;
        }

        #endregion

        #region Meat & Potatos

        public void SetIsHighlighted(bool isHighlighted)
        {
            if(IsHighlighted == isHighlighted)
                return;

            IsHighlighted = isHighlighted;
            HandleIsHighlightedLogic();
        }

        private void HandleIsHighlightedLogic()
        {
            if(_useSkinnedMeshRenderer)
            {
                _skinnedMeshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
                return;
            }

            _meshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
        }

        private void DisableHighlightable()
        {
            IsHighlightable = false;
            SetIsHighlighted(false);
        }

        #endregion

        #region Getters

        public NpcBehaviour GetNpcBehaviour()
        {
            return _npcBehaviour;
        }

        public HighlightableData GetHighlightableData()
        {
            EntityType entityType;
            string name;

            if(EntityType == EntityType.Interactable)
            {
                entityType = EntityType.Npc;
                return new HighlightableData(entityType, this.name, _nameBubbleOffest);
            }

            if(EntityType == EntityType.Npc && _npcBehaviour != null)
            {
                entityType = EntityType.Npc;
                name = _npcBehaviour.Name;
                return new HighlightableData(entityType, name, _nameBubbleOffest);
            }

            if(EntityType == EntityType.Enemy && _enemyView != null)
            {
                entityType = EntityType.Enemy;
                name = _enemyView.Name;
                int currentHP = _enemyView.CurrentHP;
                int maxHP = _enemyView.MaxHP;
                int nameBgSize = _enemyView.EvolutionLevel;
                float scaleMultiplier = _enemyView.ScaleMultiplier;
                return new HighlightableData(entityType, name, _nameBubbleOffest,
                                             currentHP, maxHP, nameBgSize, scaleMultiplier);
            }

            if(EntityType == EntityType.Loot && _lootView != null)
            {
                entityType = EntityType.Loot;
                name = _lootView.Name;
                var lootCategory = _lootView.LootCategory;
                var goldAmount = _lootView.Amount;
                return new HighlightableData(entityType, name, _nameBubbleOffest, lootCategory, goldAmount);
            }

            return null;
        }

        #endregion
    }
}
