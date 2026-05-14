using UnityEngine;

public class ConveyorBlock : IOBlock
{
    private IOFace processingFace;

    private IOFace[] inputFaces;

    private int nextInputIndex;

    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Input;

        rightFace.faceType = FaceType.Input;

        inputFaces = new IOFace[]
        {
            leftFace,
            backFace,
            rightFace
        };
    }

    protected override bool CanProcess()
    {
        if (frontFace.HasItem)
            return false;

        for (int i = 0; i < inputFaces.Length; i++)
        {
            int index =
                (nextInputIndex + i)
                % inputFaces.Length;

            IOFace face =
                inputFaces[index];

            if (face.HasItem)
            {
                processingFace = face;

                nextInputIndex =
                    (index + 1)
                    % inputFaces.Length;

                return true;
            }
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
