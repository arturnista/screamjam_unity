using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;
using TMPro;

public class Ritual : MonoBehaviour
{
    
    [SerializeField] private GenericDictionary<ItemData, Transform> _itemPositions;

    [SerializeField] private GameObject _pickupText;

    private PlayerInventory _inventory;
    private int _itemsPlaced;

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
        
        if (!_itemPositions[data].gameObject.activeSelf)
        {
            _itemsPlaced += 1;
        }

        _itemPositions[data].gameObject.SetActive(true);

        if (_itemsPlaced == 2)
        {
            GameObject.FindObjectOfType<EnemyMovement>().StartAttack();
        }
        else if (_itemsPlaced == _itemPositions.Count)
        {
            GameObject.FindObjectOfType<EnemyHealth>().SetVulnerable();
            GameObject.FindObjectOfType<EnemyMovement>().StartAttack();
            UIMessage.Instance.ShowMessage("The monster is weaken", 10f, Color.red);
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _inventory = collider.GetComponent<PlayerInventory>();
            UpdateText(_inventory);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _pickupText.SetActive(false);
        }
    }

    public void UpdateText(PlayerInventory inventory)
    {
        var item = inventory.GetItem(_itemPositions.Keys.ToList());
        if (item)
        {
            _pickupText.GetComponent<TextMeshPro>().text = $"Press E to place {item.Name}";
            _pickupText.SetActive(true);
        }
        else
        {
            _pickupText.SetActive(false);
        }
    }
}
