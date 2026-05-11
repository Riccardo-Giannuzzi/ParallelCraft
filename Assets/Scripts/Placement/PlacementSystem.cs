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
        if (inputManager.TryGetSelectedMapPosition(out Vector3 position))
        {
            Vector3Int gridPosition = grid.WorldToCell(position);

            indicator.SetActive(true);
            cellIndicator.SetActive(true);

            indicator.transform.position = position;
            cellIndicator.transform.position =
                grid.CellToWorld(gridPosition);
        }
        else
        {
            indicator.SetActive(false);
            cellIndicator.SetActive(false);
        }
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
        if (!inputManager.TryGetSelectedMapPosition(out Vector3 position))
        {
            return;
        }

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
