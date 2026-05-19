using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles connecting and disconnecting IOBlocks when they are placed or removed.
/// </summary>
public class ConnectionSystem : MonoBehaviour
{
    private readonly Vector3Int[] directions =
    {
        Vector3Int.forward,
        Vector3Int.back,
        Vector3Int.left,
        Vector3Int.right
    };

    /// <summary>
    /// Checks the neighbors of the given block and connects them if they are IOBlocks.
    /// </summary>
    /// <param name="block">The IOBlock to connect to its neighbors.</param>
    /// <param name="placedBlocks">Dictionary of all placed blocks in the world.</param>
    public void ConnectBlock(
        IOBlock block,
        Dictionary<Vector3Int, PlaceableBlock> placedBlocks)
        {
            foreach (Vector3Int dir in directions)
                CheckNeighbor(block, dir, placedBlocks);
        }

    /// <summary>
    /// Checks a single neighbor in the given direction and connects it if it is an IOBlock.
    /// </summary>
    /// <param name="block">The IOBlock to check neighbors for.</param>
    /// <param name="dir">The direction to check.</param>
    /// <param name="placedObjects">Dictionary of all placed blocks in the world.</param>
    private void CheckNeighbor(
        IOBlock block,
        Vector3Int dir,
        Dictionary<Vector3Int, PlaceableBlock> placedObjects)
        {
            Vector3Int neighborPos = block.GridPosition + dir;
            
            if (!placedObjects.TryGetValue(neighborPos, out PlaceableBlock neighborObj))
                return;

            IOBlock neighbor = neighborObj.GetComponent<IOBlock>();

            if (neighbor == null)
                return;

            IOFace myFace = block.GetFaceFromWorldDirection(dir);
            IOFace neighborFace = neighbor.GetFaceFromWorldDirection(-dir);

            if (myFace == null || neighborFace == null)
                return;

            myFace.Connect(neighborFace);
        }

    /// <summary>
    /// Disconnects all faces of the given block. Should be called when a block is removed.
    /// </summary>
    /// <param name="block">The IOBlock to disconnect.</param>
    public void DisconnectBlock(IOBlock block)
    {
        block.frontFace.Disconnect();
        block.backFace.Disconnect();
        block.leftFace.Disconnect();
        block.rightFace.Disconnect();
    }
}