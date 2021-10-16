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

    private void Awake()
    {
        _pickupText.SetActive(false);

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _data.Image;

        _textMesh = _pickupText.GetComponent<TextMeshPro>();
        _textMesh.text = $"{_data.Name}\n<size=75%>E to pick up</size>";
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
