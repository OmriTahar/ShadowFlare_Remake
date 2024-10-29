using ShadowFlareRemake.Enemies;
using ShadowFlareRemake.NPC;
using UnityEngine;

namespace ShadowFlareRemake.Behaviours
{
    public class HighlightableBehaviour : MonoBehaviour
    {
        public bool IsHighlightable { get; private set; } = true;
        public bool IsHighlighted { get; private set; }
        public bool IsEnemy { get; private set; }
        public bool IsNpc { get; private set; }

        [Header("References")]
        [SerializeField] private Transform _canvasTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private GameObject _nameHolder;

        [Header("Parent Views")]
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private NpcView _npcView;

        [Header("Settings")]
        [SerializeField] private float _highlightIntensity = 0.2f;
        [SerializeField] private bool _useSkinnedMeshRenderer;

        private const string _highlightableTag = "Highlightable";

        private Camera _mainCamera;
        private Color _color;
        private Color _highlightColor;
        private bool _isAllowedToShowName = true;

        #region MonoBehaviour

        private void Start()
        {
            InitTag();
            CacheMainCamera();
            SetIsEnemyOrNpc();
            InitColors();
            SetEnemyCanvasHeight();
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

        private void SetIsEnemyOrNpc()
        {
            IsEnemy = _enemyView != null;
            IsNpc = _npcView != null;
        }

        private void SetEnemyCanvasHeight()
        {
            if(!IsEnemy)
                return;

            var currentCanvasLocalPos = _canvasTransform.localPosition;
            var enemyScaleMultiplier = _enemyView.GetEnemyScaleMultiplier();
            var newY = currentCanvasLocalPos.y * enemyScaleMultiplier;
            var newCanvasPos = new Vector3(currentCanvasLocalPos.x, newY, currentCanvasLocalPos.z);
            _canvasTransform.localPosition = newCanvasPos;
        }

        private void CacheMainCamera()
        {
            _mainCamera = Camera.main;
        }

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
            if(IsHighlighted)
                FaceCanvasAccordingToTheCamera();

            var isNameActive = _isAllowedToShowName? IsHighlighted : false;
            _nameHolder.gameObject.SetActive(isNameActive);


            if(_useSkinnedMeshRenderer)
            {
                _skinnedMeshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
                return;
            }

            _meshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
        }

        public void SetIsAllowedToShowName(bool isAllowedToShowName)
        {
            _isAllowedToShowName = isAllowedToShowName;

            if(IsHighlighted)
                _nameHolder.gameObject.SetActive(isAllowedToShowName);
        }

        private void DisableHighlightable()
        {
            IsHighlightable = false;
            SetIsHighlighted(false);
        }

        #endregion

        #region Helpers

        public void FaceCanvasAccordingToTheCamera()
        {
            Vector3 direction = _mainCamera.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(-direction);
            _canvasTransform.rotation = rotation;
        }

        public NpcView GetNpcView()
        {
            return _npcView;
        }

        #endregion
    }
}
