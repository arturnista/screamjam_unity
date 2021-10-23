using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRandomizer : MonoBehaviour
{

    [SerializeField] private InventoryItem _itemPrefab;
    [SerializeField] private Transform _entitiesSpawn;
    
    private Dictionary<ItemData, List<Transform>> _itemPlaces;

    private Transform _playerPoint;
    private Transform _enemyPoint;

    private void Awake()
    {
        RandomizeItems();
        RandomizePlayer();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = _playerPoint.position;
    }

    private void RandomizeItems()
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

    private void RandomizePlayer()
    {
        int child = Random.Range(0, _entitiesSpawn.childCount);
        _playerPoint = _entitiesSpawn.GetChild(child);
    }

    public Vector3 RandomizeEnemy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        _enemyPoint = _entitiesSpawn.GetChild(0);
        for (int i = 1; i < _entitiesSpawn.childCount; i++)
        {
            var point = _entitiesSpawn.GetChild(i);
            if (Vector3.Distance(player.transform.position, _enemyPoint.position) < Vector3.Distance(point.position, player.transform.position))
            {
                _enemyPoint = point;
            }
        }
        
        return _enemyPoint.position;
    }

}
