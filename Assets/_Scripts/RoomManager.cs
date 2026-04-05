using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string roomCode = "Map1";
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPoint;

    [Space]
    [SerializeField] private GameObject roomCamera;
    private string currentName;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetName(string _name)
    {
        currentName = _name;
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Joining lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joining or creating room");
        PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions: null, typedLobby: null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room. Spawning Player");

        PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation);
        roomCamera.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = currentName;
    }

}
