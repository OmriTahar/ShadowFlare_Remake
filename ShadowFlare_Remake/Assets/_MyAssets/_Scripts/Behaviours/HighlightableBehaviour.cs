using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.NPC;
using UnityEngine;

namespace ShadowFlareRemake.Behaviours
{
    public class HighlightableBehaviour : MonoBehaviour
    {
        public bool IsHighlightable { get; private set; } = true;
        public bool IsHighlighted { get; private set; }

        [Header("References")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private GameObject _nameHolder;

        [Header("Parent Views")]
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private NpcView _npcView;

        [Header("Settings")]
        [SerializeField] private float _highlightIntensity = 0.2f;
        [SerializeField] private bool _useSkinnedMeshRenderer;
        [SerializeField] private bool _isDisplayingNameUponHover;

        private Color _color;
        private Color _highlightColor;

        private bool _isAllowedToShowName = true;

        private const string _highlightableTag = "Highlightable";

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
            if(_isAllowedToShowName)
            {
                _nameHolder.gameObject.SetActive(IsHighlighted);
            }
            else
            {
                _nameHolder.gameObject.SetActive(false);
            }

            if(_useSkinnedMeshRenderer)
            {
                _skinnedMeshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
                return;
            }

            _meshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
        }

        public void SetIsShowingName(bool isShowingName)
        {
            _nameHolder.gameObject.SetActive(isShowingName);
        }

        public void SetIsAllowedToShowName(bool isAllowedToShowName)
        {
            _isAllowedToShowName = isAllowedToShowName;
        }

        public void SetIsSpeechBubbleEnabled()
        {
            // Continue from here 
        }

        private void DisableHighlightable()
        {
            IsHighlightable = false;
            SetIsHighlighted(false);
        }

        public NpcView GetNpcView()
        {
            return _npcView;
        }

        #endregion
    }
}
