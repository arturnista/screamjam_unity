using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletForce = 10f;
    [SerializeField] Light2D fireLight;
    [Header("Sprite")]
    [SerializeField] private GameObject _musketArms;
    [Header("SFX")]
    [SerializeField] private List<AudioClip> _fireSounds;

    private EntityData _entityData;
    private AudioSource _audioSource;

    private void Awake()
    {
        _entityData = GetComponentInParent<EntityData>();
        _audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnEnable()
    {
        firePoint.gameObject.SetActive(true);
        _musketArms.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        fireLight.gameObject.SetActive(false);
        firePoint.gameObject.SetActive(false);
        _musketArms.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        var lookingDirection = _entityData.LookDirection.normalized;
        // firePoint.position = lookingDirection + (transform.position - new Vector3(0f, .5f));
        float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;
        _musketArms.transform.eulerAngles = new Vector3(0, 0, angle) + _entityData.RotationSum;
        // firePoint.eulerAngles = new Vector3(0, 0, angle - 90f);
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); 
        Rigidbody2D bulletRb =  bullet.GetComponent<Rigidbody2D>();

        bulletRb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        if (_fireSounds.Count > 0)
        {
            _audioSource.PlayOneShot(_fireSounds[Random.Range(0, _fireSounds.Count)]);
        }

        StopAllCoroutines();
        StartCoroutine(FireLightCoroutine());
    }

    private IEnumerator FireLightCoroutine()
    {
        fireLight.intensity = 1.4f;
        fireLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        
        float lightTime = .5f;
        for (float i = 0; i < lightTime; i += Time.deltaTime)
        {
            float t = i / lightTime;
            fireLight.intensity = Mathf.Lerp(1.4f, 0f, t);
            yield return null;
        }
        fireLight.gameObject.SetActive(false);
    }

}
