using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

public class EnemyHealth : EntityHealth
{

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;
    
    [SerializeField] private ParticleSystem _vulnerableEffect;

    private bool _isVulnerable;
    private int _health = 2;

    public void SetVulnerable()
    {
        _damageEffect = _vulnerableEffect;
        _isVulnerable = true;
    }

    public override void DealDamage(int damage)
    {
        base.DealDamage(damage);
        if (_isVulnerable)
        {
            _health -= 1;
            if (_health <= 0)
            {
                // gameObject.SetActive(false);
                GetComponent<EnemyMovement>().StopMovement();
                OnDeath?.Invoke();
            }
        }
    }

}
