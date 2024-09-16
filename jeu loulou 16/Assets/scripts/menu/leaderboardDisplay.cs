using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;
using Unity.VisualScripting;

public class leaderboardDisplay : MonoBehaviour
{
    // secret leaderboard key: a23c87185143668f4c864eb78f04d06cfc561ce146f2d42baaccdec02f769ab96c0893a64e9721bc3a68d1e73cddc4adca2a16f5c644917839d7d0a411bcab2d4584a9035469f4fe694bd7b0411eea50f4a1790882ac2523171a745521e42cdb7d24f96e31a3fad8d6e8f59b6fbe47f6a57ce85113dd041e3a89a9ed0d1f358c
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;
    [SerializeField] private AudioSource mainTheme;
    public GameObject background;
    public GameObject start;
    public GameObject end;
    public float speed;
    public bool going;
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

    void Awake()
    {
        going = true;
        GetLeaderboard();
        Invoke("refresh", 60);
    }
    void Start()
    {
        going = true;
        GetLeaderboard();
        Invoke("refresh", 60);
        mainTheme.Pause();
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
