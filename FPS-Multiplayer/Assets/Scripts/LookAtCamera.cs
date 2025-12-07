using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    void Awake()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
    }
}
