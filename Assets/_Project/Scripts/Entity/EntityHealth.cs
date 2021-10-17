using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    
    public delegate void DamageHandler(float damage);
    public event DamageHandler OnDamage;

    [SerializeField] private List<AudioClip> _damageSounds = default;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public virtual void DealDamage(float damage)
    {
        _audioSource.PlayOneShot(_damageSounds[Random.Range(0, _damageSounds.Count)]);
        OnDamage?.Invoke(damage);
    }

}
