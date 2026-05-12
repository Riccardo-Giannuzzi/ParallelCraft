using UnityEngine;

public abstract class BlockBase : MonoBehaviour
{
    [Header("Base Settings")]
    public string blockID;
    public int health = 100;

    protected virtual void Start()
    {
        // Logic that every single block should execute
        Debug.Log($"Block {blockID} initialized.");
    }

    //generic method for interaction
    public virtual void OnInteract()
    {
        Debug.Log("Interacting with base block.");
    }
}