using UnityEngine;

public abstract class IOBlock : PlaceableBlock
{

    [Header("Block Faces")]
    public IOFace frontFace;

    public IOFace backFace;

    public IOFace leftFace;

    public IOFace rightFace;

    public IOFace GetFaceFromWorldDirection(Vector3Int dir)
    {
        switch (rotation)
        {
            case BlockRotation.North:

                if (dir == Vector3Int.forward)
                    return frontFace;

                if (dir == Vector3Int.back)
                    return backFace;

                if (dir == Vector3Int.left)
                    return leftFace;

                if (dir == Vector3Int.right)
                    return rightFace;

                break;

            case BlockRotation.East:

                if (dir == Vector3Int.right)
                    return frontFace;

                if (dir == Vector3Int.left)
                    return backFace;

                if (dir == Vector3Int.forward)
                    return leftFace;

                if (dir == Vector3Int.back)
                    return rightFace;

                break;

            case BlockRotation.South:

                if (dir == Vector3Int.back)
                    return frontFace;

                if (dir == Vector3Int.forward)
                    return backFace;

                if (dir == Vector3Int.right)
                    return leftFace;

                if (dir == Vector3Int.left)
                    return rightFace;

                break;

            case BlockRotation.West:

                if (dir == Vector3Int.left)
                    return frontFace;

                if (dir == Vector3Int.right)
                    return backFace;

                if (dir == Vector3Int.back)
                    return leftFace;

                if (dir == Vector3Int.forward)
                    return rightFace;

                break;
        }

        return null;
    }

}