using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReloadDisplay : MonoBehaviour
{

    [SerializeField] private Image _image;

    [SerializeField] private PlayerAttack _playerAttack;


    private void Update()
    {
        if (_playerAttack.AttackTime > 0f)
        {
            _image.fillAmount = 1f - Mathf.Clamp01(_playerAttack.AttackTime / _playerAttack.ReloadCooldown);
        }
        else
        {
            _image.fillAmount = 1f;
        }
    }

}
