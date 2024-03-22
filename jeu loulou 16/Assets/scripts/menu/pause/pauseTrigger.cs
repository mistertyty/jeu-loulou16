using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pauseTrigger : MonoBehaviour
{
    public KeyCode pausekey = KeyCode.Escape;
    public KeyCode pausekey2 = KeyCode.P;
    public static bool isPaused;
    public GameObject deathscreen;
    public GameObject pauseMenu;
    public GameObject mainpause;
    public GameObject areyousure;
    public GameObject end;
    public GameObject upload;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    public float maxgameTime;
    private float currentGameTime;
    private int intGameTime;
    public bool gameEnd;
    public int score;
    public GameObject screenScore;
    public GameObject screenTime;

    // Update is called once per frame
    private void Start()
    {
        score = 0;
        gameEnd = false;
        currentGameTime = maxgameTime;
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        time.text = intGameTime.ToString();
        scoreDisplay.text = score.ToString();
        if (!gameEnd)
            updateTime();

        if ((Input.GetKeyDown(pausekey) || Input.GetKeyDown(pausekey2)) && !gameEnd)
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            else
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                pauseMenu.SetActive(true);
                deathscreen.SetActive(false);
                areyousure.SetActive(false);
                end.SetActive(false);
                upload.SetActive(false);
                mainpause.SetActive(true);
            }
                
            isPaused = !isPaused;
        }
    }

    private void updateTime()
    {
        currentGameTime -= Time.deltaTime;
        intGameTime = (int) currentGameTime;
        if (currentGameTime <= 0)
        {
            intGameTime = 0;
            gameEnd = true;
            endgame();
        }

    }

    private void endgame()
    {
        screenScore.SetActive(false);
        screenTime.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(true);
        deathscreen.SetActive(false);
        areyousure.SetActive(false);
        end.SetActive(true);
        upload.SetActive(false);
        mainpause.SetActive(false);
    }
}
