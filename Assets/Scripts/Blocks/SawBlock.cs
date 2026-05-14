using UnityEngine;

public class SawBlock : IOBlock
{
    [Header("Items")]
    [SerializeField]
    private Item woodLog;

    [SerializeField]
    private Item wood;

    [SerializeField]
    private Item stick;

    private void Awake()
    {
        processDelay = 3f;

        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Closed;

        rightFace.faceType = FaceType.Closed;
    }

    protected override bool CanProcess()
    {
        if (frontFace.HasItem)
            return false;

        if (!backFace.HasItem)
            return false;

        Item input = backFace.currentItem;

        return
            input == woodLog
            ||
            input == wood;
    }

    protected override void CompleteProcess()
    {
        Item input = backFace.currentItem;

        if (input == woodLog)
        {
            frontFace.currentItem = wood;
        }
        else if (input == wood)
        {
            frontFace.currentItem = stick;
        }

        backFace.currentItem = null;
    }

}
