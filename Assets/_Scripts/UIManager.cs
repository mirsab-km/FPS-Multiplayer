using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;

    [Header("End Screen")]
    public GameObject endScreen;
    public TextMeshProUGUI winnerText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ShowEndScreen(string winnerName)
    {
        endScreen.SetActive(true);
        winnerText.text = "Winner: " + winnerName;
    }
}