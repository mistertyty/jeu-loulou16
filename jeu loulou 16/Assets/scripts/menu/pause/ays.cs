using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ays : MonoBehaviour
{
    public GameObject pauseMenu;
    public void yes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void no()
    {
        gameObject.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
