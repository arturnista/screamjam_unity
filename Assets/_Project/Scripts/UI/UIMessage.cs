using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMessage : MonoBehaviour
{

    private static UIMessage s_Instance;
    public static UIMessage Instance => s_Instance;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        s_Instance = this;

        _text = GetComponent<TextMeshProUGUI>();
        _text.enabled = false;
        _text.text = "";
    }
    
    public void ShowMessage(string message)
    {
        ShowMessage(message, 3f);
    }
    
    public void ShowMessage(string message, float time)
    {
        ShowMessage(message, time, Color.white);
    }

    public void ShowMessage(string message, float time, Color color)
    {
        StopAllCoroutines();
        StartCoroutine(MessageCoroutine(message, time, color));
    }
    
    public void Clear()
    {
        StopAllCoroutines();

        _text.enabled = false;
        _text.text = "";
    }

    private IEnumerator MessageCoroutine(string message, float time, Color color)
    {
        _text.text = message;
        _text.enabled = true;
        _text.color = color;

        yield return new WaitForSeconds(time);

        Clear();
    }

}
