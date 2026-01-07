using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
