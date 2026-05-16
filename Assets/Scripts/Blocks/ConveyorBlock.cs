using System.Collections;
using UnityEngine;

public class ConveyorBlock : IOBlock
{
    private IOFace processingFace;

    private IOFace[] inputFaces;

    private int nextInputIndex;

    [SerializeField]
    private float pushDelay = 1f;

    private float pushTimer;

    private bool isPushing;

    [SerializeField]
    private ItemVisual backItem;
    [SerializeField]
    private ItemVisual leftItem;
    [SerializeField]
    private ItemVisual rightItem;
    [SerializeField]
    private ItemVisual centerItem;


    [Header("Animation Points")]
    [SerializeField]
    private Transform frontPosition;

    [SerializeField]
    private Transform centerPosition;

    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Input;

        rightFace.faceType = FaceType.Input;

        inputFaces = new IOFace[]
        {
            leftFace,
            backFace,
            rightFace
        };
    }

    protected override void Update()
    {
        base.Update();

        UpdateVisuals();
    }

    protected override bool CanProcess()
    {
        if (frontFace.HasItem)
            return false;

        for (int i = 0; i < inputFaces.Length; i++)
        {
            int index =
                (nextInputIndex + i)
                % inputFaces.Length;

            IOFace face =
                inputFaces[index];

            if (face.HasItem)
            {
                processingFace = face;

                nextInputIndex =
                    (index + 1)
                    % inputFaces.Length;

                return true;
            }
        }

        return false;
    }

    protected override void CompleteProcess()
    {
        if (processingFace == null)
            return;

        frontFace.currentItem =
            processingFace.currentItem;

        processingFace.currentItem = null;

        processingFace = null;
    }

    protected override void TryStartProcess()
    {
        if (isProcessing)
            return;

        if (!CanProcess())
            return;

        isProcessing = true;

        processTimer = processDelay;

        StartCoroutine(
            AnimateProcessing(processingFace)
        );
    }

    protected override void TryPushOutputs()
    {
        if (isPushing)
            return;

        if (!frontFace.HasItem)
            return;

        if (frontFace.connectedFace == null)
            return;

        if (!frontFace.connectedFace.CanReceiveItem())
            return;

        StartCoroutine(AnimatePush());
    }

    private void UpdateVisuals()
    {
        UpdateVisual(backItem, backFace);

        UpdateVisual(leftItem, leftFace);

        UpdateVisual(rightItem, rightFace);

        UpdateVisual(centerItem, frontFace);
    }

    private void UpdateVisual(ItemVisual visual,IOFace face)
    {
        visual.SetItem(face.currentItem);
    }

    private IEnumerator AnimateProcessing(IOFace inputFace)
    {
        ItemVisual visual = GetInputVisual(inputFace);

        Vector3 start = visual.transform.position;

        yield return MoveVisual(
            visual.transform,
            start,
            centerPosition.position,
            processDelay
        );

        visual.transform.position = start;
    }

    private IEnumerator AnimatePush()
    {
        isPushing = true;

        Vector3 start =
            centerItem.transform.position;

        yield return MoveVisual(
            centerItem.transform,
            start,
            frontPosition.position,
            pushDelay
        );

        centerItem.transform.position = start;

        TryPushFace(frontFace);

        pushTimer = pushDelay;

        isPushing = false;
    }

    private ItemVisual GetInputVisual(IOFace face)
    {
        if (face == backFace)
            return backItem;

        if (face == leftFace)
            return leftItem;

        if (face == rightFace)
            return rightItem;

        return null;
    }


    private IEnumerator MoveVisual(
    Transform visual,
    Vector3 start,
    Vector3 end,
    float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            visual.position = Vector3.Lerp(start, end, t);

            yield return null;
        }

        visual.position = end;
    }
}
