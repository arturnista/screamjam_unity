using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _finalText;
    [SerializeField] private GameObject _anyKey;

    private bool _isSkipText;
    private bool _skipIsAllowed;
    private Action _skipAction;

    private void Awake()
    {
        _finalText.transform.parent.gameObject.SetActive(false);
        _skipIsAllowed = false;
    }

    private void OnEnable()
    {
        GameObject.FindObjectOfType<PlayerHealth>().OnDeath += () => {
            StartCoroutine(LoseGameCoroutine());
        };
    }

    private void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        var enemy = GameObject.FindObjectOfType<EnemyHealth>();
        enemy.OnDeath += () => {
            GameObject.FindObjectOfType<PlayerHealth>().Clear();
            StartCoroutine(WinGameCoroutine());
        };
        enemy.gameObject.SetActive(false);

        bool skip = false;
        _skipAction = () => {
            Time.timeScale = 1f;
            _finalText.transform.parent.gameObject.SetActive(false);
            skip = true;
        };

        yield return FinalTextCoroutine(
            "<b>The ritual poem</b>",
            "",
            "A <b>Sword</b> so I can fight",
            "A <b>Trinket</b> to channel my might",
            "A <b>Blood Bottle</b> to know who's beside me",
            "A <b>Music Box</b> so I can't forget thee",
            "And the last three <b>Old Coins</b>",
            "So you can finally be free of your debt"
        );
        yield return new WaitUntil(() => skip);

        UIMessage.Instance.ShowMessage("Press <b>Q</b> to switch between Lamp and Gun", 5f);
        yield return new WaitForSeconds(5f);
        UIMessage.Instance.ShowMessage("Press <b>Left Mouse</b> to fire your Gun", 5f);
        yield return new WaitForSeconds(5f);
        UIMessage.Instance.ShowMessage("Survive the beast", 5f);
        yield return new WaitForSeconds(5f);
        UIMessage.Instance.ShowMessage("Complete the ritual", 5f);
        yield return new WaitForSeconds(5f);
        
        var position = FindObjectOfType<MapRandomizer>().RandomizeEnemy();
        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
    }

    private IEnumerator WinGameCoroutine()
    {
        _skipAction = () => {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1f;
        };

        for (float i = 1f; i > 0f; i -= Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }

        yield return FinalTextCoroutine(
            "You completed the ritual,",
            "and hunted down the beast.",
            "Did you finally ended the horror your village was suffering?",
            "The beast was dead?",
            "",
            "<b><color=red>You hope so</color></b>"
        );
    }

    private IEnumerator LoseGameCoroutine()
    {
        UIMessage.Instance.ShowMessage("You are dead", 10f, Color.red);
        for (float i = 1f; i > 0f; i -= Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }

        _skipAction = () => {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1f;
        };
        _skipIsAllowed = true;
    }

    private IEnumerator FinalTextCoroutine(params string[] messages)
    {
        UIMessage.Instance.Clear();
        
        _isSkipText = false;
        _anyKey.SetActive(false);
        _finalText.text = "";
        _finalText.transform.parent.gameObject.SetActive(true);
        
        foreach (var item in messages)
        {
            var message = item;
            string tagString = "";
            for (int i = 0; i < message.Length; i++)
            {
                if (tagString.Length > 0 || message[i] == '<')
                {
                    tagString += message[i].ToString();
                    if (message[i] == '>')
                    {
                        _finalText.text += tagString;
                        tagString = "";
                    }
                }
                else
                {
                    _finalText.text += message[i];
                    if (!_isSkipText) yield return new WaitForSecondsRealtime(.07f);
                }
            }
            _finalText.text += "\n";
        }

        yield return new WaitForSecondsRealtime(1f);
        _anyKey.SetActive(true);
        _skipIsAllowed = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (_skipIsAllowed)
            {
                _skipIsAllowed = false;
                _skipAction();
            }
            else if (!_isSkipText)
            {
                _isSkipText = true;
            }
        }
    }

}
