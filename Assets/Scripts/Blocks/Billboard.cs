using UnityEngine;

/// <summary>
/// A simple billboard script that makes the object always face the main camera, while keeping its vertical orientation fixed. UI elements or sprites shall always be visible to the player.
/// </summary>
public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// In the LateUpdate method, the script calculates the direction from the billboard to the camera, ignoring any vertical component to maintain a flat orientation.
    /// </summary>
    private void LateUpdate()
    {
        Vector3 direction = mainCamera.transform.forward;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
            transform.forward = direction.normalized;
    }
}