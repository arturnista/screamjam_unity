using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class UILink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void OpenLink(string link);
#else
    private void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
#endif

    [SerializeField] private string _link;

    private TextMeshProUGUI _textMesh;
    private Button _button;

    void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => OpenLink(_link));
    }

    public void OnPointerEnter(PointerEventData data)
    {
        _textMesh.fontStyle = FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData data)
    {
        _textMesh.fontStyle = FontStyles.Normal;
    }

}
