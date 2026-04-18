using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using WebSocketSharp;
public class ChatManager : MonoBehaviourPun
{
    public TextMeshProUGUI chatText;
    public TMP_InputField chatInput;

    private bool isInputFieldToggled;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (!isInputFieldToggled)
            {
                isInputFieldToggled = true;
                chatInput.Select();
                chatInput.ActivateInputField();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInputFieldToggled)
            {
                isInputFieldToggled = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        bool isReturnButtonDown = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
        if (isReturnButtonDown && isInputFieldToggled && !chatInput.text.IsNullOrEmpty())
        {
            photonView.RPC("RPC_SentChatMessage", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName, chatInput.text);

            //Send messages
            chatInput.text = "";
            isInputFieldToggled = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    [PunRPC]
    public void RPC_SentChatMessage(string _playerName, string _chatessage)
    {
        string messageToAdd = "<b>" + _playerName + ":</b> " + _chatessage;
        chatText.text = messageToAdd + "\n" + chatText.text;
    }
}
