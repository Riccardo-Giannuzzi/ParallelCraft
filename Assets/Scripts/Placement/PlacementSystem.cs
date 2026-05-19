using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private PlayerInventory inventory;
    [SerializeField]
    private ConnectionSystem connectionSystem;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip placeSound;

    [SerializeField]
    private AudioClip breakSound;

    private Dictionary<Vector3Int, PlaceableBlock> placedBlocks = new Dictionary<Vector3Int, PlaceableBlock>();

    private void Awake()
    {
        PlaceableBlock[] existingBlocks =
            FindObjectsByType<PlaceableBlock>(
                FindObjectsSortMode.None
            );

        foreach (PlaceableBlock block in existingBlocks)
        {
            RegisterBlock(block);
        }
    }

    private void Start()
    {
        inputManager.OnClicked += ClickEvent;
    }

    private void Update()
{
    if (inputManager.TryGetSelectedMapPosition(out Vector3 position))
    {
        Vector3Int gridPosition = grid.WorldToCell(position);

        cellIndicator.SetActive(true);

        cellIndicator.transform.position = grid.GetCellCenterWorld(gridPosition);

        BlockRotation rotation = GetPlayerRotation();

        Quaternion worldRotation = RotationToQuaternion(rotation);


        cellIndicator.transform.rotation = worldRotation;
    }
    else
    {
        cellIndicator.SetActive(false);
    }
}

    private void ClickEvent()
    {
        HotbarSlot currentItem = inventory.GetCurrentSlot();

        if (currentItem == null)
            return;

        if (currentItem.itemType == ItemType.Tool)
        {
            BreakBlock();
            return;
        }

        if (currentItem.itemType == ItemType.Placeable)
        {
            PlaceBlock(currentItem);
        }

    }

    public void RegisterBlock(
    PlaceableBlock block)
    {
        Vector3Int gridPosition =
            grid.WorldToCell(
                block.transform.position
            );

        block.GridPosition =
            gridPosition;

        placedBlocks.Add(
            gridPosition,
            block
        );

        IOBlock ioBlock =
            block.GetComponent<IOBlock>();

        if (ioBlock != null)
        {
            connectionSystem.ConnectBlock(
                ioBlock,
                placedBlocks
            );
        }
    }

    private void PlaceBlock(HotbarSlot slot)
    {
        if (!slot.unlocked)
            return;

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
            slot.placeablePrefab,
            worldPosition,
            worldRotation
        );

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(placeSound);

        PlaceableBlock placeable =
            placedObject.GetComponent<PlaceableBlock>();

        placeable.GridPosition = gridPosition;
        placeable.rotation = rotation;

        RegisterBlock(placeable);
    }

    private void BreakBlock()
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

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(breakSound);
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

    public List<IOBlock> GetAllIOBlocks()
    {
        List<IOBlock> blocks =
            new List<IOBlock>();

        foreach (PlaceableBlock block in placedBlocks.Values)
        {
            IOBlock ioBlock =
                block.GetComponent<IOBlock>();

            if (ioBlock != null)
            {
                blocks.Add(ioBlock);
            }
        }

        return blocks;
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