using UnityEngine;
using System.Collections.Generic;

public class TransporterBlock : IOBlock
{
    /// <summary>
    /// Helper class to track each individual item currently on the belt.
    /// </summary>
    private class TransportingItem
    {
        public GameObject obj;
        public Vector3 startPos;
        public float progress;
    }

    [Header("Transport Settings")]
    [Tooltip("Time in seconds to travel from start to end")]
    public float timeToCross = 1.0f; 
    [Tooltip("Scrolling speed of the belt texture")]
    public float textureSpeed = 2.0f;

    // Lists to manage multiple items and handle concurrent arrivals
    private List<TransportingItem> activeItems = new List<TransportingItem>();
    private List<TransportingItem> toRemove = new List<TransportingItem>();

    protected override void Start()
    {
        // Inherit base initialization (handles grid connections)
        base.Start(); 
        blockID = "belt_basic"; 
    }

    /// <summary>
    /// Always returns true to allow multiple items to overlap or follow each other.
    /// </summary>
    public override bool CanReceiveItem()
    {
        return true; 
    }

    /// <summary>
    /// Called when an item enters the block. 
    /// Creates a new transport tracking object for the item.
    /// </summary>
    public override void ReceiveItem(GameObject item, Transform entryPoint)
    {
        if (item == null) return;

        activeItems.Add(new TransportingItem {
            obj = item,
            startPos = entryPoint.position,
            progress = 0f
        });
    }

    void Update()
    {
        AnimateTexture();
        HandleTransport();
    }

    /// <summary>
    /// Updates the position of all items currently on the belt.
    /// </summary>
    private void HandleTransport()
    {
        toRemove.Clear();

        foreach (var tItem in activeItems)
        {
            // Safety check: if the item was destroyed elsewhere, clean up the list
            if (tItem.obj == null) 
            { 
                toRemove.Add(tItem); 
                continue; 
            }

            // Increment progress based on delta time and crossing speed
            tItem.progress += (Time.deltaTime / timeToCross);
            
            // Calculate center position to create a curved/smooth movement path
            Vector3 centerPos = transform.position;
            centerPos.y = tItem.startPos.y; // Keep vertical alignment consistent

            // 2-Phase Lerp: Start -> Center -> Output
            if (tItem.progress <= 0.5f)
            {
                // First half: moving from entry point to block center
                float t = tItem.progress * 2f;
                tItem.obj.transform.position = Vector3.Lerp(tItem.startPos, centerPos, t);
            }
            else if (tItem.progress <= 1.0f)
            {
                // Second half: moving from center to output point
                float t = (tItem.progress - 0.5f) * 2f;
                tItem.obj.transform.position = Vector3.Lerp(centerPos, output.position, t);
            }
            else
            {
                // Movement complete: finalize position at output
                tItem.obj.transform.position = output.position;

                // Attempt to pass the item to the next block in the chain
                if (nextBlock != null && nextBlock.CanReceiveItem())
                {
                    nextBlock.ReceiveItem(tItem.obj, this.output);
                    toRemove.Add(tItem);
                }
                // Note: If the next block is full, the item stays at the output point
            }
        }

        // Remove items that have finished their journey or were destroyed
        foreach (var item in toRemove) 
        {
            activeItems.Remove(item);
        }
    }

    /// <summary>
    /// Simple texture offset animation to visually simulate belt movement.
    /// </summary>
    private void AnimateTexture()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            float offset = Time.time * textureSpeed * 0.25f;
            // Assumes the texture is mapped to the Y axis for scrolling
            rend.material.mainTextureOffset = new Vector2(0, -offset);
        }
    }
}