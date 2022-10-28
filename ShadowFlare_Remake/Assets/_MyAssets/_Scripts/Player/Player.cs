using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Unit _unit;

    [Header("Movement Settings")]
    [SerializeField] private int _movementSpeed = 5;
    [SerializeField] private int _rotationSpeed = 5;

    public int MovementSpeed { get { return _movementSpeed; } }
    public int RotationSpeed { get { return _rotationSpeed; } }


    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }
}
