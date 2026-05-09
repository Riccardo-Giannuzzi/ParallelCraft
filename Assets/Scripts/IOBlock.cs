using UnityEngine;
using System.Collections.Generic;

public abstract class IOBlock : BlockBase
{
    [Header("Connessioni")]
    public List<Transform> inputs = new List<Transform>(); // Inizializziamo la lista vuota
    public Transform output; 
    
    public IOBlock nextBlock; 

    // QUESTA è la funzione magica che userà il tuo amico
    public void ConnectTo(IOBlock targetBlock)
    {
        // 1. Imposto il bersaglio come mio prossimo blocco
        nextBlock = targetBlock;

        // 2. Vado nel blocco bersaglio e gli aggiungo il MIO output alla SUA lista di input
        if (targetBlock != null && !targetBlock.inputs.Contains(this.output))
        {
            targetBlock.inputs.Add(this.output);
        }
    }

    // Funzione per scollegare (utile quando distruggi un blocco sulla griglia)
    public void Disconnect()
    {
        if (nextBlock != null)
        {
            nextBlock.inputs.Remove(this.output);
            nextBlock = null;
        }
    }

    public virtual bool CanReceiveItem() 
    { 
        return false; 
    }

    public virtual void ReceiveItem(GameObject item, Transform entryPoint) 
    { 
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach(var input in inputs) if(input != null) Gizmos.DrawSphere(input.position, 0.1f);
        
        Gizmos.color = Color.red;
        if(output != null) Gizmos.DrawSphere(output.position, 0.1f);
    }
}