using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public Vector3 GetSelectedMapPosition()
    {
        Ray ray = new Ray(
            sceneCamera.transform.position,
            sceneCamera.transform.forward
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, placementLayermask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
