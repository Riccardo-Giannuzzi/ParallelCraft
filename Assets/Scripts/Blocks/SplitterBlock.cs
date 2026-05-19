using UnityEngine;



/// <summary>
/// Implementation of the splitter block
/// </summary>
public class SplitterBlock : IOBlock
{
    private IOFace[] outputFaces;

    private int nextOutputIndex;

    private IOFace selectedOutputFace;

    [SerializeField]
    private float delayPerConnection = 3f;

    /// <summary>
    /// Initializes the block's orientation, defining the input/output roles for each face and caching the output faces.
    /// </summary>
    private void Awake()
    {
        backFace.faceType = FaceType.Input;

        frontFace.faceType = FaceType.Output;

        leftFace.faceType = FaceType.Output;

        rightFace.faceType = FaceType.Output;

        outputFaces = new IOFace[] {frontFace, leftFace, rightFace};
    }

    /// <summary>
    /// Validates if the splitter can cycle an item from the input face to the next available connected output face using a round-robin logic.
    /// </summary>
    /// <returns>True if an item is ready at the input and a free output face is selected; otherwise, false.</returns>
    protected override bool CanProcess()
    {
        if (!backFace.HasItem)
            return false;

        for (int i = 0; i < outputFaces.Length; i++)
        {
            int index = (nextOutputIndex + i) % outputFaces.Length;
            IOFace face = outputFaces[index];

            if (face.HasItem)
                continue;

            selectedOutputFace = face;
            nextOutputIndex = (index + 1) % outputFaces.Length;
            processDelay = GetConnectedOutputCount() * delayPerConnection;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Finalizes the split operation by transferring the item from the input back face to the chosen target output face.
    /// </summary>
    protected override void CompleteProcess()
    {
        if (selectedOutputFace == null)
            return;

        selectedOutputFace.currentItem = backFace.currentItem;
        backFace.currentItem = null;
        selectedOutputFace = null;
    }

    /// <summary>
    /// Counts the number of active external connections attached to the output faces.
    /// </summary>
    /// <returns>The total number of connected output faces, capped at a minimum value of 1.</returns>
    private int GetConnectedOutputCount()
    {
        int count = 0;

        foreach (IOFace face in outputFaces)
        {
            if (face.IsConnected)
                count++;
        }

        return Mathf.Max(count, 1);
    }
}