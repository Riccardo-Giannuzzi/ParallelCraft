using UnityEngine;


/// <summary>
/// IO base block abstract class
/// </summary>
public abstract class IOBlock : PlaceableBlock
{
    [SerializeField]
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

    /// <summary>
    /// Tries to start the processing of the item. Doesn't work if it's already processing something else.
    /// </summary>
    protected virtual void TryStartProcess()
    {
        //evaluates if an item is already being processed
        if (isProcessing)
            return;

        //method implemented by inherited classes
        if (!CanProcess())
            return;

        isProcessing = true;

        processTimer = processDelay;
    }

    /// <summary>
    /// Processes the item for a set amount of time, depending on the specific implementation of IOBlock.
    /// </summary>
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

    /// <summary>
    /// Tries to push all the output faces to connected inputs(if there are any).
    /// </summary>
    protected virtual void TryPushOutputs()
    {
        TryPushFace(frontFace);
        TryPushFace(backFace);
        TryPushFace(leftFace);
        TryPushFace(rightFace);
    }

    /// <summary>
    /// Checks if the chosen face is of output type, has an item, and tries to the next connected input face, if it's not null
    /// </summary>
    /// <param name="face">The other face</param>
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

    /// <summary>
    /// Returns all block faces as an array
    /// </summary>
    /// <returns>Array with all face references</returns> 
    protected IOFace[] GetAllFaces()
    {
        return new IOFace[] {frontFace, backFace, leftFace, rightFace};
    }

    /// <summary>
    /// Returns all block input faces as an array
    /// </summary>
    /// <returns>Array with all face references</returns> 
    protected IOFace[] GetInputFaces()
    {
        return System.Array.FindAll(GetAllFaces(), face => face.faceType == FaceType.Input);
    }

    /// <summary>
    /// Returns all block output faces as an array.
    /// </summary>
    /// <returns>Array with all face references</returns> 
    protected IOFace[] GetOutputFaces()
    {
        return System.Array.FindAll(GetAllFaces(), face => face.faceType == FaceType.Output);
    }

    /// <summary>
    /// Returns all block output faces as an array.
    /// </summary>
    /// <returns>Array with all face references</returns> 
    protected IOFace GetOutputFace()
    {
        foreach (IOFace face in GetAllFaces())
        {
            if (face.faceType == FaceType.Output)
                return face;
        }

        return null;
    }

    public virtual void ClearItems()
    {
        foreach (IOFace face in GetAllFaces())
        {
            face.currentItem = null;
        }

        isProcessing = false;

        processTimer = 0f;
    }

    /// <summary>
    /// Returns the face given a direction.
    /// </summary>
    /// <returns>selected Face</returns> 
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