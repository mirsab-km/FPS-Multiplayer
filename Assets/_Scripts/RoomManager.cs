using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set; }
    [SerializeField] private string roomCode = "Map1";
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spawnPoints;

    [Space]
    [SerializeField] private GameObject roomCamera;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject timerPanel;
    private string currentName;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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

    public Vector3 GetRandomSpawnPos()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        return spawnPoint.position;
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

        PhotonNetwork.Instantiate(player.name, GetRandomSpawnPos(), Quaternion.identity);
        roomCamera.SetActive(false);
        chatPanel.SetActive(true);
        timerPanel.SetActive(true);
        PhotonNetwork.LocalPlayer.NickName = currentName;
    }

    public void RespawnPlayer()
    {
        PhotonNetwork.Instantiate(player.name, GetRandomSpawnPos(), Quaternion.identity);
    }
}
