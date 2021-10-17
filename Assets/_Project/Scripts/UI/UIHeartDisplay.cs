using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeartDisplay : MonoBehaviour
{

    [SerializeField] private AudioClip _heartBeat01;
    [SerializeField] private AudioClip _heartBeat02;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
    }
    
    public void AnimHeartBeat01()
    {
        _audioSource.PlayOneShot(_heartBeat01);
    }
    
    public void AnimHeartBeat02()
    {
        _audioSource.PlayOneShot(_heartBeat02);
    }

}
