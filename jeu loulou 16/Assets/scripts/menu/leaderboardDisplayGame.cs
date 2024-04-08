using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;
using Unity.VisualScripting;

public class leaderboardDisplayGame : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;
    private string publicLeaderboardKey = "23caa230eddc2e0f8157d91ff3bebe9f4c3242b0383aff64b268611d4278b011";

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
}
