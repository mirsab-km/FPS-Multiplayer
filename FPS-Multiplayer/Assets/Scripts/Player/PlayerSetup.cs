using UnityEngine;
using Photon.Pun;
using Cinemachine;
using StarterAssets;
using TMPro;
using System.Linq;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private ThirdPersonController movementScript;
    [SerializeField] private TextMeshPro playerNameText;

    void Start()
    {
        if (photonView.IsMine)
        {
            SetupFollowCamera();

            // Enable movement only for the local player
            if (movementScript != null)
            {
                movementScript.enabled = true;
            }
            else
            {
                Debug.LogError("Movement script reference missing on player prefab!");
            }
        }
        else
        {
            // Disable movement for remote players
            if (movementScript != null)
            {
                movementScript.enabled = false;
            }
        }

        if (playerNameText != null)
        {
            playerNameText.gameObject.SetActive(!photonView.IsMine);
            playerNameText.text = photonView.Owner.NickName;
        }
    }

    private void SetupFollowCamera()
    {
        CinemachineVirtualCamera[] allCams = FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None);

        CinemachineVirtualCamera followCam = allCams.FirstOrDefault(cam => cam.gameObject.name == "PlayerFollowCamera");

        if (followCam != null && playerCameraRoot != null)
        {
            followCam.Follow = playerCameraRoot;
        }
        else
        {
            // Log an error if either the camera or the root is missing
            Debug.LogError("Error setting up camera: Either 'PlayerFollowCamera' not found or 'playerCameraRoot' reference is missing.");
        }
    }
}