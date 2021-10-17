using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] private List<AudioClip> _pickupSounds = default;

    private List<ItemData> _inventory;
    private AudioSource _audioSource;

    private void Awake()
    {
        _inventory = new List<ItemData>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
        }
    }

    private void CheckItem()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach(var item in colliders)
        {
            if (item.gameObject.CompareTag("Item"))
            {
                var inventoryItem = item.GetComponent<InventoryItem>();   
                var itemData = inventoryItem.PickUp();
                _inventory.Add(itemData);

                if (_pickupSounds.Count > 0)
                {
                    _audioSource.PlayOneShot(_pickupSounds[Random.Range(0, _pickupSounds.Count)]);
                }
            }
            else if (item.gameObject.CompareTag("Ritual"))
            {
                var ritual = item.GetComponent<Ritual>();   
                if (_inventory.Count > 0)
                {
                    var itemWasPlaced = ritual.PlaceItem(_inventory[0]);
                    if (itemWasPlaced) _inventory.Remove(_inventory[0]);
                }
            }
        }
    }

}
