using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundPosition : MonoBehaviour
{

    private Transform _target;
    private AudioSource _audioSource;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        var xDiff = (transform.position.x - _target.position.x) / 5f;
        _audioSource.panStereo = xDiff;
    }

}
