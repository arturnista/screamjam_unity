using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    
    public delegate void DamageHandler(float damage);
    public event DamageHandler OnDamage;

    public virtual void DealDamage(float damage)
    {
        OnDamage?.Invoke(damage);
    }

}
