using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    
    public delegate void DamageHandler(int damage);
    public event DamageHandler OnDamage;

    [SerializeField] protected List<AudioClip> _damageSounds = default;
    [SerializeField] protected ParticleSystem _damageEffect;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public virtual void DealDamage(int damage)
    {
        if (_damageSounds.Count > 0)
        {
            _audioSource.PlayOneShot(_damageSounds[Random.Range(0, _damageSounds.Count)]);
        }
        if (_damageEffect != null)
        {
            _damageEffect.Play();
        }
        OnDamage?.Invoke(damage);
    }

}
