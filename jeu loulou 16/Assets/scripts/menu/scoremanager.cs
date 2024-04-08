using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class scoremanager : MonoBehaviour
{
    public pauseTrigger pauseTrigger;
    [SerializeField] private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;
    public void SubmitScore()
    {
        Debug.Log("ca marche pourtant connard");
        submitScoreEvent.Invoke(inputName.text, pauseTrigger.score);
    }
}
