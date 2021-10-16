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

    private EntityData _entityData;

    private void Awake()
    {
        _entityData = GetComponentInParent<EntityData>();
    }

    private void OnEnable()
    {
        firePoint.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        fireLight.gameObject.SetActive(false);
        firePoint.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        firePoint.position = _entityData.LookDirection + transform.position;
        float angle = Mathf.Atan2(_entityData.LookDirection.y, _entityData.LookDirection.x) * Mathf.Rad2Deg - 90f;
        firePoint.eulerAngles = new Vector3(0, 0, angle);
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
