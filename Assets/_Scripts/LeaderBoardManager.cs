using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
public class LeaderBoardManager : MonoBehaviour
{
    public GameObject leaderboardUI;
    [Space]
    public Transform playerItemPrefabParent;
    public GameObject playerItemPrefab;

    void Start()
    {
        InvokeRepeating(nameof(UpdateLeaderboard), 0.5f, 0.5f);
    }

    void Update()
    {
        leaderboardUI.SetActive(Input.GetKey(KeyCode.Tab));
    }

    private void UpdateLeaderboard()
    {
        for (int i = playerItemPrefabParent.childCount - 1; i >= 0; i--)
        {
            Destroy(playerItemPrefabParent.GetChild(i).gameObject);
        }

        var sortedPlayerList = 
        (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        foreach (var _player in sortedPlayerList)
        {
            GameObject playerItem = Instantiate(playerItemPrefab, playerItemPrefabParent);

            string _nickname = _player.NickName;
            
            bool _isMe = _player.UserId == PhotonNetwork.LocalPlayer.UserId;
            if(_isMe) _nickname = "<color=#8CC924>" + _nickname + "</color>";

            playerItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _nickname;
            playerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _player.GetScore().ToString();

            int _kills = 0;
            int _deaths = 0;
            if (_player.CustomProperties["Kills"] != null) _kills = (int)_player.CustomProperties["Kills"];
            if (_player.CustomProperties["Deaths"] != null) _deaths = (int)_player.CustomProperties["Deaths"];

            playerItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _kills + " / " + _deaths;
        }
    }
}