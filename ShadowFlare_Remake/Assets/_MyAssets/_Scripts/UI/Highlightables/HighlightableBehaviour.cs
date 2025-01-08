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

        [Header("Parent Views")]
        [SerializeField] private NpcView _npcView;
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private LootView _lootView; 

        [Header("Settings")]
        [SerializeField] private int _nameBubbleUiOffest;
        [SerializeField] private float _highlightIntensity = 0.2f;
        [SerializeField] private bool _useSkinnedMeshRenderer;
        [field: SerializeField] public EntityType EntityType { get; private set; }

        private const string _highlightableTag = "Highlightable";

        private Color _color;
        private Color _highlightColor;

        #region MonoBehaviour

        private void Start()
        {
            InitTag();
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

        private void InitTag()
        {
            if(gameObject.tag != _highlightableTag)
            {
                gameObject.tag = _highlightableTag;
            }
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

        #region Helpers

        public NpcView GetNpcView()
        {
            return _npcView;
        }

        public HighlightableData GetHighlightableData()
        {
            EntityType entityType;
            string name;

            if(EntityType == EntityType.Npc && _npcView != null)
            {
                entityType = EntityType.Npc;
                name = _npcView.Name;
                return new HighlightableData(entityType, name, _nameBubbleUiOffest);
            }

            if(EntityType == EntityType.Enemy && _enemyView != null)
            {
                entityType = EntityType.Enemy;
                name = _enemyView.Name;
                int currentHP = _enemyView.CurrentHP;
                int maxHP = _enemyView.MaxHP;
                int evolutionLevel = _enemyView.EvolutionLevel;
                float scaleMultiplier = _enemyView.ScaleMultiplier;
                return new HighlightableData(entityType, name, _nameBubbleUiOffest,
                                             currentHP, maxHP, evolutionLevel, scaleMultiplier);
            }

            if(EntityType == EntityType.Loot && _lootView != null)
            {
                entityType = EntityType.Loot;
                name = _lootView.Name;
                var lootCategory = _lootView.LootCategory;
                var goldAmount = _lootView.Amount;
                return new HighlightableData(entityType, name, _nameBubbleUiOffest, lootCategory, goldAmount);
            }

            return null;
        }

        #endregion
    }
}
