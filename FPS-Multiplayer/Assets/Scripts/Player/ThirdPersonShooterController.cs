using UnityEngine;
using Cinemachine;
using StarterAssets;
using System.Linq;
using Photon.Pun;

public class ThirdPersonShooterController : MonoBehaviourPunCallbacks
{
    private CinemachineVirtualCamera playerAimCamera;
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private float normalSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 0.5f;
    private ThirdPersonController thirdPersonControllerScript;
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonControllerScript = GetComponent<ThirdPersonController>();
    }
    void Start()
    {
        if (photonView.IsMine)
        {
            SetupAimCamera(); // only local player sets up aim camera
        }
    }
    void Update()
    {
        if (!photonView.IsMine) return;
        if (playerAimCamera == null) return;

        if (starterAssetsInputs.aim)
        {
            playerAimCamera.gameObject.SetActive(true);
            thirdPersonControllerScript.SetSensitivity(aimSensitivity);
        }
        else
        {
            playerAimCamera.gameObject.SetActive(false);
            thirdPersonControllerScript.SetSensitivity(normalSensitivity);
        }
    }

    private void SetupAimCamera()
    {
        CinemachineVirtualCamera[] allCam = FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None);
        CinemachineVirtualCamera foundAimCam = allCam.FirstOrDefault(cam => cam.gameObject.name == "PlayerAimCamera");

        if (foundAimCam != null && playerCameraRoot != null)
        {
            // Assign the found camera to your private field
            playerAimCamera = foundAimCam;

            // Assign the follow target
            playerAimCamera.Follow = playerCameraRoot;

            Debug.Log("Successfully assigned PlayerAimCamera targets.");
        }
        else
        {
            Debug.LogError("Camera setup failed: Ensure 'PlayerAimCamera' exists in the scene and 'playerCameraRoot' is linked in the Inspector.");
        }
    }
}
