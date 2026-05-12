using UnityEngine;

/// <summary>
/// Handles the creation and initial routing of items within the IO chain.
/// </summary>
public class ItemSpawner : IOBlock
{
    [Header("Settings")]
    public GameObject itemPrefab;
    public KeyCode spawnKey = KeyCode.Space;

    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnAndSend();
        }
    }

    /// <summary>
    /// Instantiates the item and transfers ownership to the next block if available.
    /// </summary>
    private void SpawnAndSend()
    {
        if (nextBlock != null && nextBlock.CanReceiveItem())
        {
            // Instantiate at the output transform and transfer to the next link in the chain
            GameObject newItem = Instantiate(itemPrefab, output.position, Quaternion.identity);
            nextBlock.ReceiveItem(newItem, output);
            
            Debug.Log("Item successfully spawned and dispatched.");
        }
        else
        {
            Debug.LogWarning("Spawn failed: Next block is missing or currently occupied.");
        }
    }

    /// <summary>
    /// Spawner blocks act as sources only and cannot receive external items.
    /// </summary>
    public override bool CanReceiveItem() => false;
}