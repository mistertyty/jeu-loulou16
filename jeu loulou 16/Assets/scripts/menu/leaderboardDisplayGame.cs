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
