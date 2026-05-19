using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles placing and breaking blocks in the world, as well as tracking placed blocks for easy access by other systems. Also handles the cell indicator that shows where a block will be placed.
/// </summary>
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

    // Dictionary to track placed blocks by their grid position for easy access by the connection system and other systems that need to query blocks in the world.
    private Dictionary<Vector3Int, PlaceableBlock> placedBlocks = new Dictionary<Vector3Int, PlaceableBlock>();

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        PlaceableBlock[] existingBlocks = FindObjectsByType<PlaceableBlock>(FindObjectsSortMode.None);
        foreach (PlaceableBlock block in existingBlocks)
            RegisterBlock(block);
    }

    private void Start()
    {
        inputManager.OnClicked += ClickEvent;
    }

    private void Update()
    {
        if (inputManager.TryGetSelectedMapPosition(out Vector3 position))
        {
            // Snap the position to the grid to get the correct cell for the indicator.
            Vector3Int gridPosition = grid.WorldToCell(position);

            cellIndicator.SetActive(true);

            //rotate the indicator to match the player's current rotation so that it shows the correct orientation of the block to be placed.
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

    /// <summary>
    /// Handles the player's click input for placing and breaking blocks.
    /// If the player is holding a placeable item, it will attempt to place it. If they are holding a tool, it will attempt to break the targeted block.
    /// </summary>
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

    /// <summary>
    /// Registers a newly placed block in the placedBlocks dictionary and connects it to its neighbors if it is an IOBlock. 
    /// </summary>
    /// <param name="block">Newly placed block</param>
    public void RegisterBlock(PlaceableBlock block)
    {
        Vector3Int gridPosition = grid.WorldToCell(block.transform.position);
        block.GridPosition = gridPosition;
        placedBlocks.Add(gridPosition, block);

        IOBlock ioBlock = block.GetComponent<IOBlock>();

        if (ioBlock != null)
            connectionSystem.ConnectBlock(ioBlock,placedBlocks);
    }

    /// <summary>
    /// Attempts to place a block based on the given hotbar slot. Checks if the player can place the block at the targeted position and if so, instantiates it and registers it. Also plays a placement sound.
    /// </summary>
    /// <param name="slot">The hotbar slot corresponding to the block we want to place</param>
    private void PlaceBlock(HotbarSlot slot)
    {
        if (!slot.unlocked)
            return;

        if (!inputManager.TryGetSelectedMapPosition(out Vector3 position))
            return;

        Vector3Int gridPosition = grid.WorldToCell(position);

        // Check if there is already a block in this position. If there is, we can't place another one.
        if (placedBlocks.ContainsKey(gridPosition))
            return;

        // Calculate the world position and rotation for the block to be placed at based on the grid position and cell size.
        Vector3 worldPosition = grid.CellToWorld(gridPosition) + grid.cellSize / 2;
        BlockRotation rotation = GetPlayerRotation();
        Quaternion worldRotation = RotationToQuaternion(rotation);

        GameObject placedObject = Instantiate(slot.placeablePrefab, worldPosition, worldRotation);

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(placeSound);

        PlaceableBlock placeable = placedObject.GetComponent<PlaceableBlock>();
        placeable.GridPosition = gridPosition;
        placeable.rotation = rotation;

        RegisterBlock(placeable);
    }

    /// <summary>
    /// Attempts to break the targeted block. Checks if there is a block targeted and if so, removes it from the placedBlocks dictionary, disconnects it from its neighbors if it is an IOBlock, plays a breaking sound, and destroys the block game object.
    /// </summary>
    private void BreakBlock()
    {
        PlaceableBlock target = inputManager.GetTargetedPlaceable();

        if (target == null)
            return;

        Vector3Int gridPosition = target.GridPosition;
        placedBlocks.Remove(gridPosition);

        IOBlock ioBlock = target.GetComponent<IOBlock>();

        if (ioBlock != null)
            connectionSystem.DisconnectBlock(ioBlock);

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(breakSound);

        Destroy(target.gameObject);
    }

    /// <summary>
    /// Tries to get a block at the given grid position from the placedBlocks dictionary. 
    /// </summary>
    /// <param name="pos">position in 3d space where we want to try get a block</param>
    /// <param name="block">where to place the block reference, in case</param>
    /// <returns>True if the block is present and gets returned correctly; False otherwise</returns>
    public bool TryGetBlock(Vector3Int pos, out PlaceableBlock block)
    {
        return placedBlocks.TryGetValue(pos, out block);
    }

    /// <summary>
    /// Returns a list of all IOBlocks currently placed in the world. 
    /// </summary>
    /// <returns></returns>
    public List<IOBlock> GetAllIOBlocks()
    {
        List<IOBlock> blocks = new List<IOBlock>();

        foreach (PlaceableBlock block in placedBlocks.Values)
        {
            IOBlock ioBlock = block.GetComponent<IOBlock>();

            if (ioBlock != null)
                blocks.Add(ioBlock);
        }

        return blocks;
    }

    /// <summary>
    /// Determines the player's current facing direction and returns the corresponding BlockRotation.
    /// </summary>
    /// <returns></returns>
    private BlockRotation GetPlayerRotation()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        float dotForward = Vector3.Dot(forward, Vector3.forward);
        float dotRight = Vector3.Dot(forward, Vector3.right);

        if (Mathf.Abs(dotForward) > Mathf.Abs(dotRight))
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

    /// <summary>
    /// Converts a BlockRotation to a Quaternion for rotating the block when placing it and for rotating the cell indicator.
    /// </summary>
    /// <param name="rotation">Rotation parameter</param>
    /// <returns>the resulting Quaternion object</returns>
    private Quaternion RotationToQuaternion(BlockRotation rotation)
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