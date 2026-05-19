using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of the sink block
/// </summary>
public class SinkBlock : IOBlock
{
    [System.Serializable]
    public struct ItemCount
    {
        public Item item;
        public int count;

        public ItemCount(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }

    [Header("Consumed Items")]
    [SerializeField]
    private List<ItemCount> consumedItems = new List<ItemCount>();


    /// <summary>
    /// Assigns the respective role(Input/Output) to each face.
    /// </summary> 
    private void Awake()
    {
        frontFace.faceType = FaceType.Input;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Input;

        rightFace.faceType = FaceType.Input;
    }

    /// <summary>
    /// Checks if there's any face which contains an item ready to be processed.
    /// </summary> 
    /// <returns>True if there is a ready-to-process item; otherwise, false.</returns>
    protected override bool CanProcess()
    {
        foreach (IOFace face in GetInputFaces())
        {
            if (face.HasItem)
                return true;
        }

        return false;
    }

    /// <summary>
    /// If the item can be processed, it gets consumed by the sink block.
    /// </summary> 
    protected override void CompleteProcess()
    {
        foreach (IOFace face in GetInputFaces())
        {
            if (!face.HasItem)
                continue;

            ConsumeItem(face.currentItem);
            face.currentItem = null;
        }
    }


    /// <summary>
    /// Adds the item to an internal consumed item list, and increases item counter.
    /// </summary> 
    /// <param name="item">The item to be consumed and tracked.</param>
    private void ConsumeItem(Item item)
    {
        for (int i = 0; i < consumedItems.Count; i++)
        {
            if (consumedItems[i].item == item)
            {
                ItemCount updatedCount = consumedItems[i];
                updatedCount.count++;
                consumedItems[i] = updatedCount;
                return;
            }
        }

        consumedItems.Add(new ItemCount(item, 1));
    }

    /// <summary>
    /// Gets the count of a specific item consumed by the sink block.
    /// </summary>
    /// <param name="item">The item for which to get the count.</param>
    /// <returns>The number of instances of the item consumed.</returns>
    public int GetItemCount(Item item)
    {
        foreach (ItemCount itemCount in consumedItems)
        {
            if (itemCount.item == item)
            {
                return itemCount.count;
            }
        }

        return 0;
    }

    /// <summary>
    /// Resets the consumed item counts.
    /// </summary>
    public void ResetCounts()
    {
        consumedItems.Clear();
    }
}