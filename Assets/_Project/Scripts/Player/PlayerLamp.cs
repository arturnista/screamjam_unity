using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

public class PlayerLamp : MonoBehaviour
{
    
    [SerializeField] private GameObject _lampArm;
    private EntityData _entityData;

    private void Awake()
    {
        _entityData = GetComponentInParent<EntityData>();
    }

    private void OnEnable()
    {
        _lampArm.SetActive(true);
    }

    private void OnDisable()
    {
        _lampArm.SetActive(false);
    }

}
