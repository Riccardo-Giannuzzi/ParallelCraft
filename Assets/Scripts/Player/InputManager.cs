using System;
using UnityEngine;


/// <summary>
/// Handles player input for block placement and breaking.
/// </summary>
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

    /// <summary>
    /// Gets the world position of the block the player is currently targeting for placement, if any. 
    /// </summary>
    /// <param name="position">block coordinates in the 3d space</param>
    /// <returns>True if a block is aimed to; False otherwise</returns>
    public bool TryGetSelectedMapPosition(out Vector3 position)
    {
        //uses raycast to project aimed position
        Ray ray = new Ray(sceneCamera.transform.position, sceneCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, placementLayerMask))
        {
            position = hit.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Gets the PlaceableBlock the player is currently targeting for breaking, if any.
    /// </summary>
    /// <returns>the aimed block, if there's any</returns>
    public PlaceableBlock GetTargetedPlaceable()
    {
        Ray ray = new Ray(sceneCamera.transform.position, sceneCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, breakableLayerMask))
        {
            return hit.collider.GetComponent<PlaceableBlock>();
        }

        return null;
    }
}
