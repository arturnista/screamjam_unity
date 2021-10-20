using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{

    [SerializeField] private int _maxHealth = 2;
    private int _currentHealth;
    public int CurrentHealth => _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    
    public override void DealDamage(int damage)
    {
        _currentHealth -= damage;
        ScreenShake.Shake(.07f, 1f, 0f);
        if (_currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }

        base.DealDamage(damage);
    }

}
