using UnityEngine;

public abstract class BlockBase : MonoBehaviour
{
    [Header("Base Settings")]
    public string blockID;
    public int health = 100;

    // 'virtual' allows child classes to add their own logic to Start
    protected virtual void Start()
    {
        // Logic that every single block should execute
        Debug.Log($"Block {blockID} initialized.");
    }

    // A generic method for interaction
    public virtual void OnInteract()
    {
        Debug.Log("Interacting with base block.");
    }
}