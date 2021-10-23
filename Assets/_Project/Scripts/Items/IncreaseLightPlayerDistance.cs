using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class IncreaseLightPlayerDistance : MonoBehaviour
{
    
    [SerializeField] private float _minDistanceToActivate = 5f;
    [SerializeField] private float _distanceToMaxLight = 2f;

    private Light2D _light;
    private float _originalIntensity;
    private Transform _player;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _originalIntensity = _light.intensity;

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance > _minDistanceToActivate)
        {
            _light.enabled = false;
        }
        else
        {
            _light.enabled = true;
            float lightInt = Mathf.InverseLerp(_minDistanceToActivate, _distanceToMaxLight, distance);
            _light.intensity = _originalIntensity * lightInt;
        }
    }

}
