using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class disclaimer : MonoBehaviour
{
    private bool fadeIn;
    public CanvasGroup blackscreen;
    public GameObject intro;
    public AudioSource maintheme;
    // Start is called before the first frame update
    void Start()
    {
        fadeIn = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn == true)
        {
            blackscreen.alpha -= Time.deltaTime/2;
        }

        if (blackscreen.alpha <= 0)
        {
            fadeIn = false;
        }

        if (fadeIn == false && Input.GetMouseButtonDown(0))
        {
            Invoke("Finish", 1);
        }
    }

    private void Finish()
    {
        gameObject.SetActive(false);
        Invoke("PlayTheme", 2);
        Invoke("PlayIntro", 4);
    }

    private void PlayTheme()
    {
        maintheme.Play();
    }

    private void PlayIntro()
    {
        intro.SetActive(true);
    }
}
