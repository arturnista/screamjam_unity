using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    
    [SerializeField] private UIItem _itemPrefab;

    private PlayerInventory _inventory;

    private void Awake()
    {
        _inventory = FindObjectOfType<PlayerInventory>();
    }

    private void OnEnable()
    {
        _inventory.OnInventoryUpdate += HandleInventoryUpdate;
    }

    private void OnDisable()
    {
        _inventory.OnInventoryUpdate -= HandleInventoryUpdate;
    }

    private void HandleInventoryUpdate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (var item in _inventory.Inventory)
        {
            var itemCreated = Instantiate(_itemPrefab, transform);
            itemCreated.Construct(item);
        }
    }

}
