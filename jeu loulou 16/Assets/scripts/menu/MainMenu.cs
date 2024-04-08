using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject leaderboardMenu;
    [SerializeField] private CanvasGroup blackScreen;
    public bool transition;
    public float potato = 0;

    public AudioSource mainTheme;


    public void Start()
    {
        Time.timeScale = 1;
    }
    public void PlayGame()
    {
        transition = true;
    }

    public void Options()
    {
        gameObject.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void leaderboard()
    {
        gameObject.SetActive(false);
        leaderboardMenu.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    private void loadGame()
    {
        SceneManager.LoadScene("oli");
    }

    public void Update()
    {
        if (transition == true)
        {   
            potato += Time.deltaTime;
            Debug.Log(Time.deltaTime);
            mainTheme.volume -= Time.deltaTime/2;
            blackScreen.alpha += Time.deltaTime/2;
        }

        if (blackScreen.alpha == 1)
        {
            loadGame();
        }
    }
}
