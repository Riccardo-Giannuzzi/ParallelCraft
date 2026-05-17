using System.Collections.Generic;
using UnityEngine;

public class SinkBlock : IOBlock
{
    [Header("Consumed Items")]
    [SerializeField]
    private List<ItemCount> consumedItems = new List<ItemCount>();

    private void Awake()
    {
        frontFace.faceType = FaceType.Input;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Input;

        rightFace.faceType = FaceType.Input;
    }

    protected override bool CanProcess()
    {
        foreach (IOFace face in GetInputFaces())
        {
            if (face.HasItem)
            {
                return true;
            }
        }

        return false;
    }

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

    private void ConsumeItem(Item item)
    {
        foreach (ItemCount itemCount in consumedItems)
        {
            if (itemCount.item == item)
            {
                itemCount.count++;

                return;
            }
        }

        ItemCount newCount = new ItemCount();

        newCount.item = item;

        newCount.count = 1;

        consumedItems.Add(newCount);
    }
}