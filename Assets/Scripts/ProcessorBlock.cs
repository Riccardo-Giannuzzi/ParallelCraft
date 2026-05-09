



//DA SISTEMARE. versione vecchia




using UnityEngine;
using System.Collections;

public abstract class ProcessorBlock : IOBlock
{
    [Header("Processing Settings")]
    public float processingTime = 3.0f;
    protected bool isProcessing = false;

    protected override void Start()
    {
        base.Start();
        // ID generico che verrà sovrascritto dai figli
        if (string.IsNullOrEmpty(blockID)) blockID = "base_processor";
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Se non stiamo lavorando e l'oggetto è un item "valido"
        if (!isProcessing && collision.gameObject.CompareTag("Item"))
        {
            if (CanProcess(collision.gameObject))
            {
                StartCoroutine(ProcessRoutine(collision.gameObject));
            }
        }
    }

    // Metodo astratto: ogni macchina deciderà se può processare quell'item specifico
    protected abstract bool CanProcess(GameObject item);

    // Metodo astratto: ogni macchina deciderà cosa far uscire
    protected abstract void SpawnResult();

    protected virtual IEnumerator ProcessRoutine(GameObject inputItem)
    {
        isProcessing = true;

        // Logica di "consumo" dell'item
        Destroy(inputItem);

        // Attesa (il lavoro della macchina)
        yield return new WaitForSeconds(processingTime);

        // Creazione del risultato
        SpawnResult();

        isProcessing = false;
    }
}