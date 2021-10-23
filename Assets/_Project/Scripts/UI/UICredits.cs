using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UICredits : MonoBehaviour
{
    
    [SerializeField] private Button _backButton;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(HandleBackButtonClick);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(HandleBackButtonClick);
    }

    private void HandleBackButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
