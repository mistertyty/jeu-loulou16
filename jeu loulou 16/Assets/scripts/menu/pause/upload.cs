using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class upload : MonoBehaviour
{
    public pauseTrigger pauseTrigger;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void Update()
    {
        scoreDisplay.text = pauseTrigger.score.ToString();
    }
}
