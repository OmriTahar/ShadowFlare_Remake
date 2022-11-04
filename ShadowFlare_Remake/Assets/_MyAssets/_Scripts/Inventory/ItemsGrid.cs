using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsGrid : MonoBehaviour
{
    private const float _tileWidth = 32;
    private const float _tileHight = 32;

    private RectTransform _rectTransform;
    private Vector2 _positionOnGrid = new Vector2();
    private Vector2Int _tileGridPosition = new Vector2Int();

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        _positionOnGrid.x = mousePosition.x - _rectTransform.position.x; 
        _positionOnGrid.y = _rectTransform.position.y - mousePosition.y;

        _tileGridPosition.x = (int)(_positionOnGrid.x / _tileWidth);
        _tileGridPosition.y = (int)(_positionOnGrid.y / _tileHight);

        return _tileGridPosition;
    }
}
