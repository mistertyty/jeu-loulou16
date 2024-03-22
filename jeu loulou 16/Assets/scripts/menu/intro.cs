using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class intro : MonoBehaviour
{
    public float waitTime;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject blue;
    [SerializeField] private GameObject endpoint;
    public GameObject mainMenu;
    public GameObject black;
    public float fadeSlow;
    private bool go;
    private bool goBackground;
    public float movespeed;
    // Start is called before the first frame update
    void Start()
    {
        go = false;
        goBackground = false;
        Invoke("transitionImage", waitTime);
    }

    void Update()
    {
        if (canvasGroup.alpha > 0 && go)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeSlow ;
        }
        else if (go && !goBackground)
            Invoke("transitionBackground", waitTime/2);

        if (goBackground == true && blue.transform.position != endpoint.transform.position)
        {
            blue.transform.position = Vector3.MoveTowards(blue.transform.position, endpoint.transform.position, movespeed);
        }
        
        if (blue.transform.position == endpoint.transform.position)
        {
            mainMenu.SetActive(true);
            black.SetActive(false);
            gameObject.SetActive(false);
            blue.SetActive(false);

        }
    }
    private void transitionImage()
    {
        go = true;
    }

    private void transitionBackground()
    {
        goBackground = true;
    }
}
