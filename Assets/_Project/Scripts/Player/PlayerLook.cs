using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

public class PlayerLook : MonoBehaviour
{

    private EntityData _entityData;

    private void Awake()
    {
        _entityData = GetComponent<EntityData>();
    }
    
    private void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        _entityData.LookDirection = (mousePosition - transform.position).normalized;
    }

}
