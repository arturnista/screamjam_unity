using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    
    [SerializeField] private Image _itemImage;

    public void Construct(ItemData data)
    {
        _itemImage.sprite = data.Image;
    }

}
