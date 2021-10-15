using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletForce = 10f;
    private EntityData _entityData;

    private void Awake()
    {
        _entityData = GetComponentInParent<EntityData>();
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

    }

}
