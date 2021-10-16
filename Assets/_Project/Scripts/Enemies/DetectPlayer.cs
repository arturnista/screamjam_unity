using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;

public class DetectPlayer : MonoBehaviour
{

    public delegate void DetectPlayerHandler(Transform player);
    public event DetectPlayerHandler OnDetectPlayer;
    public event DetectPlayerHandler OnLosePlayer;

    [SerializeField] private LayerMask _detectMask;

    private EntityData _entityData;

    private Transform _player;

    private bool _detected = false;
    public bool Detected => _detected;

    private void Awake()
    {
        _entityData = GetComponentInParent<EntityData>();
    }

    private void Update()
    {
        if (_player == null) return;

        if (_detected) CheckForLose();
        else TryToDetect();
    }

    private bool CheckForLineOfSight()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, _player.position, _detectMask);
        if (hits.Length == 0) Debug.DrawLine(transform.position, _player.position, Color.green);
        else Debug.DrawLine(transform.position, _player.position, Color.red);
        return hits.Length == 0;
    }

    private void TryToDetect()
    {

        var direction = _player.position - transform.position;
        if (direction.magnitude < 2f)
        {
            if (CheckForLineOfSight())
            {
                OnDetectPlayer?.Invoke(_player);
                _detected = true;
            }
        }
        else
        {
            float angle = Vector3.Dot(_entityData.LookDirection.normalized, direction.normalized);
            if (angle > .5f)
            {
                if (CheckForLineOfSight())
                {
                    OnDetectPlayer?.Invoke(_player);
                    _detected = true;
                }
            }
        }
    }

    private void CheckForLose()
    {
        if (!CheckForLineOfSight())
        {
            OnLosePlayer?.Invoke(_player);
            _detected = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        _player = collider.transform;
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (_detected) OnLosePlayer?.Invoke(_player);
        
        _player = null;
        _detected = false;
    }

}
