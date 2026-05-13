using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using System.Linq;
using ExitGames.Client.Photon;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public static MatchManager Instance;

    [Header("Match Settings")]
    public float matchTime = 180f;

    private double startTime;
    private bool matchEnded = false;
    private bool matchStarted = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (matchEnded) return;

        if (PhotonNetwork.CurrentRoom == null) return;

        // Timer not started yet
        if (!PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("StartTime", out object value))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.timerText.text = " WAITING...";
            }

            return;
        }

        matchStarted = true;

        startTime = (double)value;

        float timePassed = (float)(PhotonNetwork.Time - startTime);
        float timeLeft = matchTime - timePassed;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;

            if (PhotonNetwork.IsMasterClient && !matchEnded)
            {
                EndMatch();
            }
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimer(timeLeft);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Only host can start match
        if (!PhotonNetwork.IsMasterClient) return;

        // Already started
        if (matchStarted) return;

        // Start only when 2 or more players are inside
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Hashtable hash = new Hashtable();
            hash["StartTime"] = PhotonNetwork.Time;

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

            matchStarted = true;

            Debug.Log("Match Started!");
        }
    }

    void EndMatch()
    {
        matchEnded = true;

        Player winner = PhotonNetwork.PlayerList
            .OrderByDescending(p => p.GetScore())
            .FirstOrDefault();

        string winnerName = winner != null ? winner.NickName : "No One";

        photonView.RPC("RPC_EndMatch", RpcTarget.All, winnerName);
    }

    [PunRPC]
    void RPC_EndMatch(string winnerName)
    {
        matchEnded = true;

        Time.timeScale = 0f;

        UIManager.Instance.UpdateTimer(0f);
        UIManager.Instance.ShowEndScreen(winnerName);
    }
}