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

    private Dictionary<Vector3Int, PlaceableBlock> placedBlocks = new Dictionary<Vector3Int, PlaceableBlock>();

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
        ToolData currentItem = inventory.GetCurrentItem();

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

    private void PlaceBlock(ToolData item)
    {
        if (!inputManager.TryGetSelectedMapPosition(out Vector3 position))
        {
            return;
        }

        Vector3Int gridPosition =
            grid.WorldToCell(position);

        if (placedBlocks.ContainsKey(gridPosition))
        {
            return;
        }

        Vector3 worldPosition =
            grid.CellToWorld(gridPosition)
            + grid.cellSize / 2;

        BlockRotation rotation =
            GetPlayerRotation();

        Quaternion worldRotation =
            RotationToQuaternion(rotation);

        GameObject placedObject = Instantiate(
            item.placeablePrefab,
            worldPosition,
            worldRotation
        );

        PlaceableBlock placeable =
            placedObject.GetComponent<PlaceableBlock>();

        placeable.GridPosition = gridPosition;
        placeable.rotation = rotation;

        placedBlocks.Add(
            gridPosition,
            placeable
        );

        IOBlock ioBlock =
            placedObject.GetComponent<IOBlock>();

        if (ioBlock != null)
        {
            connectionSystem.ConnectBlock(
                ioBlock,
                placedBlocks
            );
        }
    }

    private void BreakStructure()
    {
        PlaceableBlock target =
            inputManager.GetTargetedPlaceable();

        if (target == null)
            return;

        Vector3Int gridPosition = target.GridPosition;

        placedBlocks.Remove(gridPosition);

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
        return placedBlocks.TryGetValue(
            pos,
            out block);
    }

    private BlockRotation GetPlayerRotation()
    {
        Vector3 forward =
            Camera.main.transform.forward;

        forward.y = 0;

        forward.Normalize();

        float dotForward =
            Vector3.Dot(forward, Vector3.forward);

        float dotRight =
            Vector3.Dot(forward, Vector3.right);

        if (Mathf.Abs(dotForward) >
            Mathf.Abs(dotRight))
        {
            if (dotForward > 0)
                return BlockRotation.North;
            else
                return BlockRotation.South;
        }
        else
        {
            if (dotRight > 0)
                return BlockRotation.East;
            else
                return BlockRotation.West;
        }
    }

    private Quaternion RotationToQuaternion(
    BlockRotation rotation)
    {
        switch (rotation)
        {
            case BlockRotation.North:
                return Quaternion.Euler(0, 0, 0);

            case BlockRotation.East:
                return Quaternion.Euler(0, 90, 0);

            case BlockRotation.South:
                return Quaternion.Euler(0, 180, 0);

            case BlockRotation.West:
                return Quaternion.Euler(0, 270, 0);
        }

        return Quaternion.identity;
    }
}