using UnityEngine;
using Cinemachine;
using StarterAssets;
using System.Linq;
using Photon.Pun;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviourPunCallbacks
{
    private CinemachineVirtualCamera playerAimCamera;
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private float normalSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 0.5f;
    private ThirdPersonController thirdPersonControllerScript;
    [SerializeField] private float raycastDistance = 999f;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
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

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint); //starts from camera and pass through screen center
        if (Physics.Raycast(ray, out RaycastHit raycastHit, raycastDistance, aimColliderMask)) 
        {
            debugTransform.position = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            playerAimCamera.gameObject.SetActive(true);
            thirdPersonControllerScript.SetSensitivity(aimSensitivity);
            thirdPersonControllerScript.SetRotateOnMove(false);

            Vector3 worldAimTarget = raycastHit.point;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            playerAimCamera.gameObject.SetActive(false);
            thirdPersonControllerScript.SetSensitivity(normalSensitivity);
            thirdPersonControllerScript.SetRotateOnMove(true);
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
