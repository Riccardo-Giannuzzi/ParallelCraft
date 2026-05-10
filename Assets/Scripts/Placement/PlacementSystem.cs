using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    private void Update()
    {
        Vector3 position = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(position);
        indicator.transform.position = position;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
