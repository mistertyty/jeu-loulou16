using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject leaderboardMenu;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    
}
