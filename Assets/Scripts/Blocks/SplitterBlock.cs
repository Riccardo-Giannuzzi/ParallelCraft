using UnityEngine;

public class SplitterBlock : IOBlock
{
    private IOFace[] outputFaces;

    private int nextOutputIndex;

    private IOFace selectedOutputFace;

    [SerializeField]
    private float delayPerConnection = 3f;

    private void Awake()
    {
        backFace.faceType = FaceType.Input;

        frontFace.faceType = FaceType.Output;

        leftFace.faceType = FaceType.Output;

        rightFace.faceType = FaceType.Output;

        outputFaces = new IOFace[]
        {
            frontFace,
            leftFace,
            rightFace
        };
    }

    protected override bool CanProcess()
    {
        if (!backFace.HasItem)
            return false;

        for (int i = 0; i < outputFaces.Length; i++)
        {
            int index =
                (nextOutputIndex + i)
                % outputFaces.Length;

            IOFace face =
                outputFaces[index];

            // Output occupied
            if (face.HasItem)
                continue;

            selectedOutputFace = face;

            nextOutputIndex =
                (index + 1)
                % outputFaces.Length;

            processDelay =
                GetConnectedOutputCount()
                * delayPerConnection;

            return true;
        }

        return false;
    }

    protected override void CompleteProcess()
    {
        if (selectedOutputFace == null)
            return;

        selectedOutputFace.currentItem = backFace.currentItem;

        backFace.currentItem = null;

        selectedOutputFace = null;
    }

    private int GetConnectedOutputCount()
    {
        int count = 0;

        foreach (IOFace face in outputFaces)
        {
            if (face.IsConnected)
            {
                count++;
            }
        }

        return Mathf.Max(count, 1);
    }
}