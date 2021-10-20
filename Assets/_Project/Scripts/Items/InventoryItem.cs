using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    
    [SerializeField] private GameObject _pickupText;
    [SerializeField] private ItemData _data;

    private SpriteRenderer _spriteRenderer;
    private TextMeshPro _textMesh;

    private bool _isConstructed;

    private void Awake()
    {
        _pickupText.SetActive(false);
    }

    public void Construct(ItemData data)
    {
        _isConstructed = true;
        _data = data;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _data.Image;

        _textMesh = _pickupText.GetComponent<TextMeshPro>();
        _textMesh.text = $"{_data.Name}\n<size=75%>E to pick up</size>";
    }

    private void Start()
    {
        if (_data != null && !_isConstructed)
        {
            Construct(_data);
        }
    }

    public ItemData PickUp()
    {
        gameObject.SetActive(false);
        return _data;
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
