using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;
using Unity.VisualScripting;

public class leaderboardDisplay : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;
    public GameObject background;
    public GameObject start;
    public GameObject end;
    public float speed;
    public bool going;
    private string publicLeaderboardKey = "9041376098ea91e7a5412cfe7fd955090262f272f9733b848ad5552741116005";

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => 
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }
        ));
    }

    void Start()
    {
        going = true;
        GetLeaderboard();
        Invoke("refresh", 60);
    }

    void refresh()
    {
        GetLeaderboard();
        Invoke("refresh", 20);
    }
    public void refreshScores()
    {
        GetLeaderboard();
    }
    
    public void setLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) => 
        {
            GetLeaderboard();
        }
        ));
    }

    void Update()
    {
        if (going == true)
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, end.transform.position, speed);
            if (background.transform.position == end.transform.position)
                going = !going;
        }

        else
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, start.transform.position, speed);
            if (background.transform.position == start.transform.position)
                going = !going;
        }
    }
}
