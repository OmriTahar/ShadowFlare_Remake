using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _movementSpeed = 5;
    [SerializeField] private int _rotationSpeed = 5;

    public int MovementSpeed { get { return _movementSpeed; } }
    public int RotationSpeed { get { return _rotationSpeed; } }

}
