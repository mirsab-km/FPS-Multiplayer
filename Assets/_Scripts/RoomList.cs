using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance { get; private set; }

    public GameObject roomManagerGameObject;
    public RoomManager roomManagerScript;

    [Header("UI")]
    public Transform roomListParent;
    public GameObject roomListItemPrefb;
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

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

    IEnumerator Start()
    {
        //Precautions
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ChangeRoomToCreateName(string _roomName)
    {
        roomManagerScript.roomCode = _roomName;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (cachedRoomList.Count <= 0)
        {
            cachedRoomList = roomList;
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < cachedRoomList.Count; i++)
                {
                    if (cachedRoomList[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            roomList[i] = room;
                        }

                        cachedRoomList = newList;
                    }
                }
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefb, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/16";

            roomItem.GetComponent<RoomItemButton>().roomName = room.Name;
        }
    }

    public void JoinRoomByName(string _name)
    {
        roomManagerScript.roomCode = _name;
        roomManagerGameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }
}