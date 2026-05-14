using UnityEngine;

public class ConveyorBlock : IOBlock
{
    private IOFace processingFace;

    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Input;

        rightFace.faceType = FaceType.Input;
    }

    protected override bool CanProcess()
    {
        if (frontFace.HasItem)
            return false;

        if (leftFace.HasItem)
        {
            processingFace = leftFace;
            return true;
        }

        if (backFace.HasItem)
        {
            processingFace = backFace;
            return true;
        }

        if (rightFace.HasItem)
        {
            processingFace = rightFace;
            return true;
        }

        return false;
    }

    protected override void CompleteProcess()
    {
        if (processingFace == null)
            return;

        frontFace.currentItem =
            processingFace.currentItem;

        processingFace.currentItem = null;

        processingFace = null;
    }

}
