using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartDisplay : MonoBehaviour
{

    [SerializeField] private Image _heartImage;
    [SerializeField] private List<Sprite> _heartsSprites;
    [Header("SFX")]
    [SerializeField] private AudioClip _heartBeat01;
    [SerializeField] private AudioClip _heartBeat02;
    
    private AudioSource _audioSource;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnEnable()
    {
        _playerHealth.OnDamage += HandleDamage;
    }

    private void HandleDamage(int damage)
    {
        int heartIndex = _playerHealth.CurrentHealth - 1;
        if (heartIndex >= 0 && heartIndex < _heartsSprites.Count)
        {
            _heartImage.sprite = _heartsSprites[heartIndex];
        }
        else
        {
            _heartImage.gameObject.SetActive(false);
        }
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
