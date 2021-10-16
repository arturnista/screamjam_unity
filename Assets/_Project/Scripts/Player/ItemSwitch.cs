using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] items = default;

    // Update is called once per frame
    void Update()
    {
        SwitchItems();
    }

    void SwitchItems()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(items[0].gameObject.activeSelf)
            {
                // Ativa a lanterna
                items[0].gameObject.SetActive(false);
                items[1].gameObject.SetActive(true);
            }
            else
            {
                // Ativa a arma
                items[0].gameObject.SetActive(true);
                items[1].gameObject.SetActive(false);
            }
        }
    }
}
