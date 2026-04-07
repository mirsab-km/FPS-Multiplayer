using UnityEngine;

public class ScopeManager : MonoBehaviour
{
    public GameObject scopeOverlay;
    public GameObject crossHair;
    public float scopeFOV;
    private float defaultFOV;

    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;
    }

    void Update()
    {
        
    }

    public void SetScopeState(bool _isScoped)
    {
        cam.fieldOfView = _isScoped ? scopeFOV : defaultFOV;
        scopeOverlay.SetActive(_isScoped);
        crossHair.SetActive(!_isScoped);
    }
}
