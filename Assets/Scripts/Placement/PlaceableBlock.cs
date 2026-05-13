using UnityEngine;

public enum BlockRotation
{
    North,
    East,
    South,
    West
}

public class PlaceableBlock : MonoBehaviour
{
    public BlockRotation rotation;
    public Vector3Int GridPosition { get; set; }

}