using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvetoryItemPlacer : MonoBehaviour
{
    
    [SerializeField] private ItemData _item;
    public ItemData Item => _item;

    private void OnValidate()
    {
        if (_item != null)
        {
            gameObject.name = $"IP_{_item.Name}";
        }
    }

}
