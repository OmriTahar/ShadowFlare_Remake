using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Interactables;
using ShadowFlareRemake.Loot;
using ShadowFlareRemake.Npc;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI.Highlightables
{
    public class HighlightableBehaviour : MonoBehaviour
    {
        public bool IsHighlightable { get; private set; } = true;
        public bool IsHighlighted { get; private set; }

        [Header("Renderers")]
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;

        [Header("Entity")]
        [SerializeField] private GameObject _entityObject;
        [field: SerializeField] public EntityType EntityType { get; private set; }

        [Header("Settings")]
        [SerializeField] private Vector2Int _nameBubbleOffest;
        [SerializeField] private float _highlightIntensity = 0.7f;

        private const string _highlightableTag = "Highlightable";

        private Interactable _interactable;
        private NpcBehaviour _npcBehaviour;
        private EnemyView _enemyView;
        private LootView _lootView;

        private Dictionary<Material, Color> _colorsDict = new();
        private Dictionary<Material, Color> _highlightedColorsDict = new();
        private bool _useSkinnedMeshRenderer;

        #region MonoBehaviour

        private void Start()
        {
            CacheEntityBehaviour();
            InitTag();
            InitRenderer();
            InitColors();
            SetColors();
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
                case EntityType.Interactable:
                    _interactable = _entityObject.GetComponent<Interactable>();
                    break;

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
            if(!gameObject.CompareTag(_highlightableTag))
            {
                gameObject.tag = _highlightableTag;
            }
        }

        private void InitRenderer()
        {
            _useSkinnedMeshRenderer = _skinnedMeshRenderers != null && _skinnedMeshRenderers.Length > 0;
        }

        private void InitColors()
        {
            if(_useSkinnedMeshRenderer)
            {
                foreach(var skinnedMeshRenderer in _skinnedMeshRenderers)
                {
                    foreach(var material in skinnedMeshRenderer.materials)
                    {
                        _colorsDict.Add(material, material.color);
                    }
                }
            }
            else
            {
                foreach(var meshRenderer in _meshRenderers)
                {
                    foreach(var material in meshRenderer.materials)
                    {
                        _colorsDict.Add(material, material.color);
                    }
                }
            }

            foreach(var pair in _colorsDict)
            {
                var color = pair.Value;
                var highlightedColoer = new Color(color.r + _highlightIntensity, color.g + _highlightIntensity, color.b + _highlightIntensity, 1);
                _highlightedColorsDict.Add(pair.Key, highlightedColoer);
            }
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

        public void HandleSetIsHighlighted(bool isHighlighted)
        {
            if(IsHighlighted == isHighlighted)
                return;

            IsHighlighted = isHighlighted;
            SetColors();
        }

        private void SetColors()
        {
            if(_useSkinnedMeshRenderer)
            {
                foreach(var skinnedMeshRenderer in _skinnedMeshRenderers)
                {
                    foreach(var material in skinnedMeshRenderer.materials)
                    {
                        material.color = IsHighlighted ? _highlightedColorsDict[material] : _colorsDict[material];
                    }
                }
                return;
            }

            foreach(var meshRenderer in _meshRenderers)
            {
                foreach(var material in meshRenderer.materials)
                {
                    material.color = IsHighlighted ? _highlightedColorsDict[material] : _colorsDict[material];
                }
            }
        }

        private void DisableHighlightable()
        {
            IsHighlightable = false;
            HandleSetIsHighlighted(false);
        }

        #endregion

        #region Getters

        public Interactable GetInteractable()
        {
            return _interactable;
        }

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
