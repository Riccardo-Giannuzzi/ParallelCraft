using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract base class for blocks that process items over a set duration.
/// Handles the lifecycle of an item from reception to transformation and dispatch.
/// </summary>
public abstract class ProcessorBlock : IOBlock
{
    [Header("Processing Settings")]
    [Tooltip("Time in seconds required to process the item.")]
    public float processingTime = 3.0f;
    
    protected bool isProcessing = false;
    protected GameObject currentProcessingItem;

    protected override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(blockID)) blockID = "base_processor";
    }

    /// <summary>
    /// Checks if the block is currently available to accept a new item.
    /// </summary>
    public override bool CanReceiveItem() => !isProcessing;

    /// <summary>
    /// Entry point for items. Initiates the process if validation passes, otherwise discards the item.
    /// </summary>
    public override void ReceiveItem(GameObject item, Transform entryPoint)
    {
        if (CanProcess(item))
        {
            Debug.Log($"[{blockID}] Item accepted for processing.");
            StartCoroutine(ProcessRoutine(item));
        }
        else
        {
            Debug.LogWarning($"[{blockID}] Item rejected: invalid type or state.");
            Destroy(item); 
        }
    }

    // Contract methods for derived classes
    protected abstract bool CanProcess(GameObject item);
    protected abstract void SpawnResult();

    /// <summary>
    /// Core logic for item handling: hides the input, waits for processingTime, 
    /// and triggers the result generation.
    /// </summary>
    protected virtual IEnumerator ProcessRoutine(GameObject inputItem)
    {
        isProcessing = true;
        currentProcessingItem = inputItem;

        // Disable and relocate item to prevent interaction during processing
        inputItem.SetActive(false); 
        inputItem.transform.position = transform.position;

        yield return new WaitForSeconds(processingTime);

        if (inputItem != null) Destroy(inputItem);
        
        SpawnResult();

        // Reset state for the next item
        isProcessing = false;
        currentProcessingItem = null;
    }
    
    /// <summary>
    /// Forwards the processed result to the next linked block in the chain.
    /// </summary>
    protected void SendToNextBlock(GameObject resultItem)
    {
        if (nextBlock != null && nextBlock.CanReceiveItem())
        {
            nextBlock.ReceiveItem(resultItem, this.output);
        }
        else
        {
            Debug.LogWarning($"[{blockID}] Could not forward result: next block is null or busy.");
        }
    }
}