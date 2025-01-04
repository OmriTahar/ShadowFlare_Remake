using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.Npc;
using UnityEngine;

namespace ShadowFlareRemake.Behaviours
{
    public class HighlightableBehaviour : MonoBehaviour
    {
        public bool IsHighlightable { get; private set; } = true;
        public bool IsHighlighted { get; private set; }

        [Header("Renderers")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

        [Header("Parent Views")]
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private NpcView _npcView;

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

        public EntityNameData GetEntityNameBubbleData()
        {
            EntityType entityType;
            string name;
            int currentHP = 0;
            int maxHP = 0;
            int evolutionLevel = 1;
            float scaleMultiplier = 1;

            if(EntityType == EntityType.Enemy && _enemyView != null)
            {
                entityType = EntityType.Enemy;
                name = _enemyView.Name;
                currentHP = _enemyView.CurrentHP;
                maxHP = _enemyView.MaxHP;
                evolutionLevel = _enemyView.EvolutionLevel;
                scaleMultiplier = _enemyView.ScaleMultiplier;

                return new EntityNameData(entityType, name, currentHP, maxHP,
                                          evolutionLevel, _nameBubbleUiOffest, scaleMultiplier);
            }

            if(EntityType == EntityType.Npc && _npcView != null)
            {
                entityType = EntityType.Npc;
                name = _npcView.Name;

                return new EntityNameData(entityType, name, currentHP, maxHP,
                                          evolutionLevel, _nameBubbleUiOffest, scaleMultiplier);
            }

            return null;
        }

        #endregion
    }
}
