using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

public class PlayerAttack : MonoBehaviour
{

    private EntityData _entityData;

    private void Awake()
    {
        _entityData = GetComponent<EntityData>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _entityData.LookDirection);
            if (hit)
            {
                var health = hit.collider.GetComponent<EntityHealth>();
                health?.DealDamage(1f);
            }
        }
    }

}
