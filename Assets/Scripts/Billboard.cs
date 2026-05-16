using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 direction =
            mainCamera.transform.forward;

        direction.y = 0f;

        transform.forward =
            direction.normalized;
    }
}