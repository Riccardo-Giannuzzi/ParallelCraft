using UnityEngine;

public class ConveyorBlock : IOBlock
{
    [SerializeField]
    private float moveDelay = 1f;

    private float timer;

    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Input;

        rightFace.faceType = FaceType.Input;
    }

    private void Update()
    {
        TryMoveInputToOutput();

        TryPushOutput();
    }

    private void TryMoveInputToOutput()
    {
        if (frontFace.HasItem)
            return;

        if (!backFace.HasItem)
            return;

        frontFace.currentItem =
            backFace.currentItem;

        backFace.currentItem = null;
    }

    private void TryPushOutput()
    {
        if (!frontFace.HasItem)
            return;

        if (frontFace.connectedFace == null)
            return;

        if (!frontFace.connectedFace.CanReceiveItem())
            return;

        frontFace.connectedFace.currentItem =
            frontFace.currentItem;

        frontFace.currentItem = null;
    }
}
