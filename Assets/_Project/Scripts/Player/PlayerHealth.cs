using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

public class PlayerHealth : EntityHealth
{

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

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
            Clear();
            OnDeath?.Invoke();
        }

        base.DealDamage(damage);
    }

    public void Clear()
    {
        Destroy(GetComponent<EntityMovement>());
        Destroy(GetComponent<ItemSwitch>());
        Destroy(GetComponentInChildren<PlayerAttack>());
        Destroy(GetComponentInChildren<PlayerLamp>());
        Destroy(GetComponentInChildren<PlayerLook>());
        Destroy(this);
    }

}
