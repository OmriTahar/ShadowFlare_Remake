using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Health & Mana")]
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private int _maxMana = 20;

    private int _currentHealth;
    private int _currentMana;

    public int Health { get { return _currentHealth; } }
    public int Mana { get { return _currentMana; } }

    public void Heal(int healthAmount = 0, int manaAmount = 0)
    {
        if (_currentHealth + healthAmount > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth += healthAmount;

        if (_currentMana + manaAmount > _maxMana)
            _currentMana = _maxMana;
        else
            _currentMana += manaAmount;
    }
}
