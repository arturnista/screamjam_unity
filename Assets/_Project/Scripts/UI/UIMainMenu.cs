using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _creditsGameButton;
    [SerializeField] private Button _quitGameButton;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(HandleStartButtonClick);
        _creditsGameButton.onClick.AddListener(HandleCreditsButtonClick);
#if UNITY_WEBGL
        Destroy(_quitGameButton.gameObject);
#else
        _quitGameButton.onClick.AddListener(HandleQuitButtonClick);
#endif
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(HandleStartButtonClick);
        _creditsGameButton.onClick.RemoveListener(HandleCreditsButtonClick);
#if !UNITY_WEBGL
        _quitGameButton.onClick.RemoveListener(HandleQuitButtonClick);
#endif
    }

    private void HandleStartButtonClick()
    {
        SceneManager.LoadScene("Game");
    }

    private void HandleCreditsButtonClick()
    {
        SceneManager.LoadScene("Credits");
    }

    private void HandleQuitButtonClick()
    {
        Application.Quit();
    }


}
