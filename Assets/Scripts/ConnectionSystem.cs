using UnityEngine;

public class ConnectionSystem : MonoBehaviour
{
    [SerializeField]
    private PlacementSystem placementSystem;

    private readonly Vector3Int[] directions =
    {
        Vector3Int.forward,
        Vector3Int.back,
        Vector3Int.left,
        Vector3Int.right
    };

    public void ConnectBlock(IOBlock block)
    {
        foreach (Vector3Int dir in directions)
        {
            CheckNeighbor(block, dir);
        }
    }

    private void CheckNeighbor(
        IOBlock block,
        Vector3Int dir)
    {
        Vector3Int neighborPos =
            block.GridPosition + dir;

        if (!placementSystem.TryGetBlock(
            neighborPos,
            out PlaceableBlock neighborObject))
        {
            return;
        }

        IOBlock neighbor =
            neighborObject.GetComponent<IOBlock>();

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