using UnityEngine;

public class SourceBlock : IOBlock
{
    [SerializeField]
    private Item generatedItem;

    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Output;

        leftFace.faceType = FaceType.Output;

        rightFace.faceType = FaceType.Output;
    }

    protected override bool CanProcess()
    {
        foreach (IOFace face in GetOutputFaces())
        {
            if (!face.HasItem)
            {
                return true;
            }
        }

        return false;
    }

    protected override void CompleteProcess()
    {
        foreach (IOFace face in GetOutputFaces())
        {
            if (!face.HasItem)
            {
                face.currentItem = generatedItem;
            }
        }
    }
}