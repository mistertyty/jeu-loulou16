using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class intro : MonoBehaviour
{
    public float waitTime;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup blackScreen;
    public GameObject mainMenu;
    public float fadeSlow;
    private bool fadeOutStart;
    private bool goBackground;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
        blackScreen.alpha = 1;
        fadeOutStart = false;
        goBackground = false;
        Invoke("fadeOut", waitTime);
    }

    void Update()
    {

        if (canvasGroup.alpha < 1 && !fadeOutStart)
        {
            canvasGroup.alpha += Time.deltaTime / fadeSlow ;
        }
        else if(canvasGroup.alpha < 1)
            Invoke("fadeOut", waitTime);


        if (canvasGroup.alpha > 0 && fadeOutStart)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeSlow ;
        }
        else if (fadeOutStart && !goBackground)
            Invoke("transitionBackground", waitTime/2);


        if (goBackground == true && blackScreen.alpha != 0)
        {
            blackScreen.alpha -= Time.deltaTime / fadeSlow;

        }
        
        if (blackScreen.alpha == 0)
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);


        }
    }
    private void fadeOut()
    {
        fadeOutStart = true;
    }

    private void transitionBackground()
    {
        goBackground = true;
    }
}
