using UnityEngine;
using System.Collections;
using Photon.Pun;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public string roomCode = "Map1";
    public GameObject playerPrefab;
    public Transform spawnPoint;
    [Space]
    public GameObject roomCamera;
    private string playerName;
    void Start()
    {
        
    }

    public void SetPlayerName(string _name)
    {
        playerName = _name;
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master server");
        Debug.Log("Joining Lobby");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        Debug.Log("Creating or joining room");

        PhotonNetwork.JoinOrCreateRoom(roomCode, new Photon.Realtime.RoomOptions(), null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        //Spawn the Player
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);

        // Disable the roomCamera by a small delay
        StartCoroutine(DisableRoomCameraDelayed());

        PhotonNetwork.LocalPlayer.NickName = playerName;
    }

    IEnumerator DisableRoomCameraDelayed()
    {
        yield return new WaitForSeconds(0.3f); // small delay so camera can render at least once
        roomCamera.SetActive(false);
    }
}
