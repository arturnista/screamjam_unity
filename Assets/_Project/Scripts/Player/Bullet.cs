using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            return;
        }
        else if (collider.gameObject.CompareTag("Enemy"))
        {
            var health = collider.gameObject.GetComponent<EntityHealth>();
            health.DealDamage(1);
        }
        Destroy(gameObject);
    }
}
