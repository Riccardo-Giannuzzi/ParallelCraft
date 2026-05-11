using UnityEngine;
using System.Collections;

public abstract class ProcessorBlock : IOBlock
{
    [Header("Processing Settings")]
    public float processingTime = 3.0f;
    protected bool isProcessing = false;
    protected GameObject currentProcessingItem;

    protected override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(blockID)) blockID = "base_processor";
    }

    public override bool CanReceiveItem()
    {
        return !isProcessing;
    }

    public override void ReceiveItem(GameObject item, Transform entryPoint)
    {
        if (CanProcess(item))
        {
            Debug.Log("Ricevuto!");
            StartCoroutine(ProcessRoutine(item));
        }
        else
        {
            Destroy(item); 
        }
    }

    protected abstract bool CanProcess(GameObject item);
    protected abstract void SpawnResult();

    protected virtual IEnumerator ProcessRoutine(GameObject inputItem)
{
    isProcessing = true;
    currentProcessingItem = inputItem;

    // Forza la disattivazione immediata per capire se il codice è arrivato qui
    inputItem.SetActive(false); 
    
    // Spostiamolo lontano per sicurezza
    inputItem.transform.position = transform.position;

    yield return new WaitForSeconds(processingTime);

    if(inputItem != null) Destroy(inputItem);
    
    SpawnResult();

    isProcessing = false;
    currentProcessingItem = null;
}
    
    protected void SendToNextBlock(GameObject resultItem)
    {
        if (nextBlock != null && nextBlock.CanReceiveItem())
        {
            nextBlock.ReceiveItem(resultItem, this.output);
        }
    }
}