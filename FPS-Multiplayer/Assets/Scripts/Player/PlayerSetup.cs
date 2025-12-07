using UnityEngine;
using Photon.Pun;
using Cinemachine;
using StarterAssets;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private ThirdPersonController movementScript;
    [SerializeField] private TextMeshPro playerNameText;

    void Start()
    {
        if (photonView.IsMine)
        {
            // Assign camera follow to the local player only
            CinemachineVirtualCamera cam = FindFirstObjectByType<CinemachineVirtualCamera>();
            cam.Follow = playerCameraRoot;

            // Enable movement only for the local player
            movementScript.enabled = true;
        }
        else
        {
            // Disable movement for remote players
            movementScript.enabled = false;
        }

        playerNameText.text = photonView.Owner.NickName;
    }
}