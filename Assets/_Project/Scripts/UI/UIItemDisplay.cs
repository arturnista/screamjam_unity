using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDisplay : MonoBehaviour
{
    
    [SerializeField] private GameObject _objectToLook;

    private bool _lastState;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _lastState = _objectToLook.activeSelf;
        _canvasGroup.alpha = _objectToLook.activeSelf ? 1f : .3f;
    }

    private void Update()
    {
        if (_lastState != _objectToLook.activeSelf)
        {
            _lastState = _objectToLook.activeSelf;
            _canvasGroup.alpha = _objectToLook.activeSelf ? 1f : .3f;
        }
    }

}
