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
    public float matchTime = 180f; // 3 minutes

    private double startTime;
    private bool matchEnded = false;

    void Awake()
    {
        Instance = this;
    }

    public override void OnJoinedRoom()
{
    if (PhotonNetwork.IsMasterClient)
    {
        Hashtable hash = new Hashtable();
        hash["StartTime"] = PhotonNetwork.Time;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
}

    void Update()
{
    if (matchEnded) return;

    if (PhotonNetwork.CurrentRoom == null) return;

    if (!PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("StartTime", out object value))
        return;

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

    void EndMatch()
    {
        matchEnded = true;

        // Find winner
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

        UIManager.Instance.UpdateTimer(0f); // Force 00:00
        UIManager.Instance.ShowEndScreen(winnerName);
    }
}