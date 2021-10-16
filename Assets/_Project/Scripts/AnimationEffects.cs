using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffects : MonoBehaviour
{

    [SerializeField] private List<AudioClip> _stepSounds = default;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void OnStep()
    {
        if (_stepSounds.Count > 0)
        {
            _audioSource.PlayOneShot(_stepSounds[Random.Range(0, _stepSounds.Count)]);
        }
    }

}
