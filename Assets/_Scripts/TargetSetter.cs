using UnityEngine;

public class TargetSetter : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        target.position = transform.position;
    }
}
