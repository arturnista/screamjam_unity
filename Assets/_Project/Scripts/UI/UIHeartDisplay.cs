using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;
using UnityEngine.UI;

public class UIHeartDisplay : MonoBehaviour
{

    [SerializeField] private Image _heartImage;
    [SerializeField] private List<Sprite> _heartsSprites;
    [Header("Ambience")]
    [SerializeField] private AudioSource _ambienceSource;
    [Header("SFX")]
    [SerializeField] private AudioClip _heartBeat01;
    [SerializeField] private AudioClip _heartBeat02;
    
    private AudioSource _audioSource;
    private PlayerHealth _playerHealth;
    private EnemyMovement _enemyMovement;
    private Animator _animator;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _enemyMovement = FindObjectOfType<EnemyMovement>();
        _animator = GetComponent<Animator>();
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

    private void LateUpdate()
    {
        _animator.speed = _enemyMovement.Tension;
        _ambienceSource.volume = .4f + (Mathf.InverseLerp(1f, 3f, _enemyMovement.Tension) * .2f);
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
