using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class end : MonoBehaviour
{
    public GameObject upload;
    public pauseTrigger pauseTrigger;
    private int counting = 0;
    public GameObject afterScore;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void uploadScore()
    {
        pauseTrigger.isPaused = true;
        Time.timeScale = 0;
        gameObject.SetActive(false);
        upload.SetActive(true);
    }

    private void Update()
    {
        scoreDisplay.text = counting.ToString();
        if (counting < pauseTrigger.score)
        {
            afterScore.SetActive(false);
            counting+=1;
        }

        else
            afterScore.SetActive(true);
    }
}
