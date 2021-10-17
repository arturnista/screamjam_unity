using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleSounds : MonoBehaviour
{

    [SerializeField] private List<AudioClip> _idleSounds = default;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIdleSoundCoroutine());
    }

    private IEnumerator PlayIdleSoundCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 10f));
            if (_idleSounds.Count > 0)
            {
                _audioSource.PlayOneShot(_idleSounds[Random.Range(0, _idleSounds.Count)]);
            }
        }
    }
}
