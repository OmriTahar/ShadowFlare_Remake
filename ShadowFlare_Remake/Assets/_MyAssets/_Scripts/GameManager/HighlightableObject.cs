using UnityEngine;

namespace ShadowFlareRemake.GameManager
{
    public class HighlightableObject : MonoBehaviour
    {
        public bool IsHighlighted { get; private set; }

        [Header("References")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private GameObject _nameHolder;

        [Header("Settings")]
        [SerializeField] private float _highlightIntensity = 0.2f;
        [SerializeField] private bool _useSkinnedMeshRenderer;

        private Color _color;
        private Color _highlightColor;

        private const string _highlightableTag = "Highlightable";

        private void Start()
        {
            InitTag();
            InitColors();
            HandleIsHighlighted();
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
            if(_useSkinnedMeshRenderer)
            {
                _color = _skinnedMeshRenderer.material.color;
            }
            else
                _color = _meshRenderer.material.color;

            _highlightColor = new Color(_color.r + _highlightIntensity, _color.g + _highlightIntensity, _color.b + _highlightIntensity, 1);
        }

        public void SetIsHighlighted(bool isHighlighted)
        {
            if(IsHighlighted == isHighlighted)
                return;

            IsHighlighted = isHighlighted;
            HandleIsHighlighted();
        }

        private void HandleIsHighlighted()
        {
            _nameHolder.gameObject.SetActive(IsHighlighted);

            if(_useSkinnedMeshRenderer)
            {
                _skinnedMeshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
                return;
            }

            _meshRenderer.material.color = IsHighlighted ? _highlightColor : _color;
        }
    }
}
