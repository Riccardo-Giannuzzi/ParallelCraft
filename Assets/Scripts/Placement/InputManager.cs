using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;
    [SerializeField]
    private LayerMask breakableLayerMask;

    public event Action OnClicked;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
    }

    public bool TryGetSelectedMapPosition(out Vector3 position)
    {
        Ray ray = new Ray(
            sceneCamera.transform.position,
            sceneCamera.transform.forward
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, placementLayerMask))
        {
            position = hit.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    public PlaceableObject GetTargetedPlaceable()
    {
        Ray ray = new Ray(
            sceneCamera.transform.position,
            sceneCamera.transform.forward
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, breakableLayerMask))
        {
            return hit.collider.GetComponent<PlaceableObject>();
        }

        return null;
    }
}
