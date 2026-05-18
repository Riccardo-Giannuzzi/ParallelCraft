using UnityEngine;

public class SourceBlock : IOBlock
{
    [SerializeField]
    private Item generatedItem;

    [SerializeField]
    private ItemVisual generatedItemVisual;

    private bool isActive;

    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Output;

        leftFace.faceType = FaceType.Output;

        rightFace.faceType = FaceType.Output;
    }

    private void Start()
    {
        generatedItemVisual.SetItem(
            generatedItem
        );
    }

    protected override bool CanProcess()
    {
        if (!isActive)
            return false;

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

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}