using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSesion : MonoBehaviour
{
    [Header("Delays")]
    [SerializeField] float deathRestartDelay = 1f;
    [SerializeField] float nextLevelDelay = 1f;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI fireText;
    [SerializeField] TextMeshProUGUI levelText;

    [Header("Images")]
    [SerializeField] Image[] hearts;
    
    [Header("Sprites")]
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    int playerLives = 3;
    int coins = 0;
    int fire = 2;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSesion>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            UpdateLevelText();
        }
        UpdateTexts();
        UpdateHeartImages();
        UpdateLevelText();
    }

    void Update() 
    {
        UpdateLevelText();
    }

    public void CollectCoin()
    {
        coins++;
        if (coins > 2)
        {
            coins = 0;
            fire++;
        }
        UpdateTexts();
    }

    public bool ThrowFire()
    {
        bool res;
        if (fire > 0)
        {
            fire--;
            res = true;
        }
        else
        {
            res = false;
        }
        UpdateTexts();
        return res;
    }

    public void DoOnDeath()
    {
        if (playerLives > 1)
        {
            playerLives--;
            UpdateHeartImages();
            ReloadFromCurrent();
        }
        else
        {
            UpdateHeartImages();
            StartCoroutine(ReloadFromStartWithDelay());
        }
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        StartCoroutine(LoadSceneWithDelay(nextLevelDelay, nextSceneIndex));
    }

    void ReloadFromStart()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void ReloadFromCurrent()
    {
        StartCoroutine(LoadSceneWithDelay(deathRestartDelay,SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadSceneWithDelay(float delay, int index)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(index);
    }

    IEnumerator ReloadFromStartWithDelay()
    {
        yield return new WaitForSecondsRealtime(deathRestartDelay);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void UpdateTexts() 
    {
        coinText.text = coins.ToString();
        fireText.text = fire.ToString();
    }

    void UpdateHeartImages()
    {
        for (int i = 0; i < 3; i++)
        {
            if (playerLives > i)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    void UpdateLevelText()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        level++;
        levelText.text = "Level: " + level.ToString();
    }
}
