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
        submitScoreEvent.Invoke(inputName.text, pauseTrigger.score);
    }
}
