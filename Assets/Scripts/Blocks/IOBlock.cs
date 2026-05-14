using UnityEngine;

public abstract class IOBlock : PlaceableBlock
{
    protected float processDelay = 1f;

    protected bool isProcessing;

    protected float processTimer;

    [Header("Block Faces")]
    public IOFace frontFace;

    public IOFace backFace;

    public IOFace leftFace;

    public IOFace rightFace;

    protected virtual void Update()
    {
        TryStartProcess();

        Process();

        TryPushOutputs();
    }

    protected abstract bool CanProcess();
    protected abstract void CompleteProcess();

    private void TryStartProcess()
    {
        if (isProcessing)
            return;

        if (!CanProcess())
            return;

        isProcessing = true;

        processTimer = processDelay;
    }

    private void Process()
    {
        if (!isProcessing)
            return;

        processTimer -= Time.deltaTime;

        if (processTimer > 0f)
            return;

        CompleteProcess();

        isProcessing = false;
    }

    protected virtual void TryPushOutputs()
    {
        TryPushFace(frontFace);
        TryPushFace(backFace);
        TryPushFace(leftFace);
        TryPushFace(rightFace);
    }

    protected void TryPushFace(IOFace face)
    {
        if (face.faceType != FaceType.Output)
            return;

        if (!face.HasItem)
            return;

        if (face.connectedFace == null)
            return;

        if (!face.connectedFace.CanReceiveItem())
            return;

        face.connectedFace.currentItem =
            face.currentItem;

        face.currentItem = null;
    }

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