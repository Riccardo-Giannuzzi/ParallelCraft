using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for blocks that handle input/output logic and connections.
/// </summary>
public abstract class IOBlock : BlockBase
{
    [Header("Connection References")]
    public List<Transform> inputs = new List<Transform>();
    public Transform output; 
    public IOBlock nextBlock; 

    protected virtual void Start()
    {
        // Ensure connection is registered at startup if pre-configured in the inspector
        if (nextBlock != null && !nextBlock.inputs.Contains(this.output))
        {
            nextBlock.inputs.Add(this.output);
        }
    }

    /// <summary>
    /// Establishes a link between this block and a target block.
    /// </summary>
    public void ConnectTo(IOBlock targetBlock)
    {
        nextBlock = targetBlock;

        if (targetBlock != null && !targetBlock.inputs.Contains(this.output))
        {
            targetBlock.inputs.Add(this.output);
        }
    }

    /// <summary>
    /// Severs the connection with the next block and updates its input list.
    /// </summary>
    public void Disconnect()
    {
        if (nextBlock != null)
        {
            nextBlock.inputs.Remove(this.output);
            nextBlock = null;
        }
    }

    public virtual bool CanReceiveItem() => false;

    public virtual void ReceiveItem(GameObject item, Transform entryPoint) { }

    protected virtual void OnDrawGizmos()
    {
        // Visual debugging for input (Blue) and output (Red) ports
        Gizmos.color = Color.blue;
        foreach(var input in inputs) if(input != null) Gizmos.DrawSphere(input.position, 0.1f);
        
        Gizmos.color = Color.red;
        if(output != null) Gizmos.DrawSphere(output.position, 0.1f);
    }
}