using UnityEngine;


/// <summary>
/// Implementation of the source block
/// </summary>
public class SourceBlock : IOBlock
{
    [SerializeField]
    private Item generatedItem;

    [SerializeField]
    private ItemVisual generatedItemVisual;

    private bool isActive;

    /// <summary>
    /// Initializes the block by configuring all directional faces (front, back, left, right) to function as outputs.
    /// </summary>
    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Output;

        leftFace.faceType = FaceType.Output;

        rightFace.faceType = FaceType.Output;
    }

    /// <summary>
    /// Binds the specified item data to the visual representation handler at the start of the game.
    /// </summary>
    private void Start()
    {
        generatedItemVisual.SetItem(generatedItem);
    }

    /// <summary>
    /// Determines whether the source block can generate an item by checking if it is active and if at least one output face is empty.
    /// </summary>
    /// <returns>True if the block is active and has an available empty output face; otherwise, false.</returns>
    protected override bool CanProcess()
    {
        if (!isActive)
            return false;

        foreach (IOFace face in GetOutputFaces())
        {
            if (!face.HasItem)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Completes the production cycle by populating any unoccupied output faces with the generated item.
    /// </summary>
    protected override void CompleteProcess()
    {
        foreach (IOFace face in GetOutputFaces())
        {
            if (!face.HasItem)
                face.currentItem = generatedItem;
        }
    }

    /// <summary>
    /// Enables the source block, allowing it to start processing and generating items.
    /// </summary>
    public void Activate()
    {
        isActive = true;
    }

    /// <summary>
    /// Disables the source block, halting any further processing and item generation.
    /// </summary>
    public void Deactivate()
    {
        isActive = false;
    }
}