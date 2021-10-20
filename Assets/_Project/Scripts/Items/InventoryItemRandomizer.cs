using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemRandomizer : MonoBehaviour
{

    [SerializeField] private InventoryItem _itemPrefab;
    
    private Dictionary<ItemData, List<Transform>> _itemPlaces;

    private void Awake()
    {
        _itemPlaces = new Dictionary<ItemData, List<Transform>>();

        foreach (var placer in FindObjectsOfType<InvetoryItemPlacer>())
        {
            if (!_itemPlaces.ContainsKey(placer.Item))
            {
                _itemPlaces.Add(placer.Item, new List<Transform>());
            }

            _itemPlaces[placer.Item].Add(placer.transform);
        }

        foreach (var placer in _itemPlaces)
        {
            var spawnPosition = placer.Value[Random.Range(0, placer.Value.Count)];
            var created = Instantiate(_itemPrefab, spawnPosition.position, spawnPosition.rotation, transform);
            created.Construct(placer.Key);
        }
    }

}
