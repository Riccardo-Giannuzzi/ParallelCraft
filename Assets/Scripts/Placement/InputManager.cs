using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClicked;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
    }

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
