using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;

    [Header("End Screen")]
    public GameObject endScreen;

    public TextMeshProUGUI winnerText;

    [Header("Winner Stats")]
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;
    public TextMeshProUGUI scoreText;

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        winnerText.text = winnerName;

        FindFirstObjectByType<LeaderBoardManager>().ShowFinalLeaderboard();

        UpdateWinnerStats(winnerName);
    }

    void UpdateWinnerStats(string winnerName)
    {
        Player winner =
            PhotonNetwork.PlayerList
            .FirstOrDefault(p => p.NickName == winnerName);

        if (winner == null) return;

        int kills = 0;
        int deaths = 0;

        if (winner.CustomProperties["Kills"] != null)
            kills = (int)winner.CustomProperties["Kills"];

        if (winner.CustomProperties["Deaths"] != null)
            deaths = (int)winner.CustomProperties["Deaths"];

        killsText.text = kills.ToString();
        deathsText.text = deaths.ToString();
        scoreText.text = winner.GetScore().ToString();
    }
}