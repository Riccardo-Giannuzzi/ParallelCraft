using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private PlayerInventory inventory;

    private Dictionary<Vector3Int, GameObject> placedObjects = new Dictionary<Vector3Int, GameObject>();

    private void Start()
    {
        inputManager.OnClicked += PlaceStructure;
    }

    private void Update()
    {
        Vector3 position = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(position);
        indicator.transform.position = position;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    private void PlaceStructure()
    {
        ItemData currentItem = inventory.GetCurrentItem();

        if (currentItem == null)
            return;

        if (currentItem.itemType == ItemType.Tool)
        {
            BreakStructure();
            return;
        }

        if (currentItem.itemType == ItemType.Placeable)
        {
            PlaceBlock(currentItem);
        }
    }

    private void PlaceBlock(ItemData item)
    {
        Vector3 position = inputManager.GetSelectedMapPosition();

        Vector3Int gridPosition = grid.WorldToCell(position);

        if (placedObjects.ContainsKey(gridPosition))
        {
            return;
        }

        Vector3 worldPosition = grid.CellToWorld(gridPosition) + grid.cellSize / 2;

        GameObject placedObject = Instantiate(
            item.placeablePrefab,
            worldPosition,
            Quaternion.identity
        );

        placedObject
            .GetComponent<PlaceableObject>()
            .GridPosition = gridPosition;

        placedObjects.Add(gridPosition, placedObject);
    }

    private void BreakStructure()
    {
        PlaceableObject target =
            inputManager.GetTargetedPlaceable();

        if (target == null)
            return;

        Vector3Int gridPosition = target.GridPosition;

        placedObjects.Remove(gridPosition);

        Destroy(target.gameObject);
    }
}
