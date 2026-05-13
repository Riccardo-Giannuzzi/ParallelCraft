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
    [SerializeField]
    private ConnectionSystem connectionSystem;

    private Dictionary<Vector3Int, GameObject> placedObjects = new Dictionary<Vector3Int, GameObject>();

    private void Start()
    {
        inputManager.OnClicked += ClickEvent;
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

    private void ClickEvent()
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
            .GetComponent<PlaceableBlock>()
            .GridPosition = gridPosition;

        placedObjects.Add(gridPosition, placedObject);

        IOBlock ioBlock = placedObject.GetComponent<IOBlock>();
        if (ioBlock != null)
        {
            connectionSystem.ConnectBlock(ioBlock);
        }

    }

    private void BreakStructure()
    {
        PlaceableBlock target =
            inputManager.GetTargetedPlaceable();

        if (target == null)
            return;

        Vector3Int gridPosition = target.GridPosition;

        placedObjects.Remove(gridPosition);

        IOBlock ioBlock =target.GetComponent<IOBlock>();

        if (ioBlock != null)
        {
            connectionSystem.DisconnectBlock(ioBlock);
        }

        Destroy(target.gameObject);
    }

    public bool TryGetBlock(
    Vector3Int pos,
    out PlaceableBlock block)
    {
        if (placedObjects.TryGetValue(pos, out GameObject obj))
        {
            block = obj.GetComponent<PlaceableBlock>();
            return true;
        }

        block = null;
        return false;
    }
}