using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaderboard : MonoBehaviour
{
    public GameObject Menu;

    public void back()
    {
        gameObject.SetActive(false);
        Menu.SetActive(true);
    }
}
