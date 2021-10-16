using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritual : MonoBehaviour
{
    
    [SerializeField] private GenericDictionary<ItemData, Transform> _itemPositions;

    [SerializeField] private GameObject _pickupText;

    private void Awake()
    {
        _pickupText.SetActive(false);
        foreach (var item in _itemPositions)
        {
            item.Value.gameObject.SetActive(false);
        }
    }

    public bool PlaceItem(ItemData data)
    {
        if (!_itemPositions.ContainsKey(data)) return false;
        _itemPositions[data].gameObject.SetActive(true);
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _pickupText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _pickupText.SetActive(false);
        }
    }
}
