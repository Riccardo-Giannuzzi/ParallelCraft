using UnityEngine;

public enum BlockRotation
{
    North,
    East,
    South,
    West
}

/// <summary>
/// Represents a block that can be placed in the world. Stores its rotation and grid position for easy access by the placement and connection systems.
/// </summary>
public class PlaceableBlock : MonoBehaviour
{
    public BlockRotation rotation;
    public Vector3Int GridPosition { get; set; }
}