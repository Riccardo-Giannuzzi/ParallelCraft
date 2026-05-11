using UnityEngine;

public class MockProcessor : ProcessorBlock
{
    [Header("Mock Settings")]
    public GameObject resultPrefab; 

    protected override void Start()
    {
        blockID = "mock_processor";
        base.Start();
    }

    protected override bool CanProcess(GameObject item)
    {
        return item != null;
    }

    protected override void SpawnResult()
    {
        GameObject result = Instantiate(resultPrefab, output.position, Quaternion.identity);
        
        SendToNextBlock(result);
    }
}