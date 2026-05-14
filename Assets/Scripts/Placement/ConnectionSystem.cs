using System.Collections.Generic;
using UnityEngine;

public class ConnectionSystem : MonoBehaviour
{
    private readonly Vector3Int[] directions =
    {
        Vector3Int.forward,
        Vector3Int.back,
        Vector3Int.left,
        Vector3Int.right
    };

    public void ConnectBlock(
        IOBlock block,
        Dictionary<Vector3Int, PlaceableBlock> placedBlocks)
    {
        foreach (Vector3Int dir in directions)
        {
            CheckNeighbor(
                block,
                dir,
                placedBlocks);
        }
    }

    private void CheckNeighbor(
        IOBlock block,
        Vector3Int dir,
        Dictionary<Vector3Int, PlaceableBlock> placedObjects)
    {
        Vector3Int neighborPos =
            block.GridPosition + dir;

        if (!placedObjects.TryGetValue(
            neighborPos,
            out PlaceableBlock neighborObj))
        {
            return;
        }

        IOBlock neighbor =
            neighborObj.GetComponent<IOBlock>();

        if (neighbor == null)
            return;

        IOFace myFace =
            block.GetFaceFromWorldDirection(dir);

        IOFace neighborFace =
            neighbor.GetFaceFromWorldDirection(-dir);

        if (myFace == null || neighborFace == null)
            return;

        myFace.Connect(neighborFace);
    }

    public void DisconnectBlock(IOBlock block)
    {
        block.frontFace.Disconnect();
        block.backFace.Disconnect();
        block.leftFace.Disconnect();
        block.rightFace.Disconnect();
    }
}