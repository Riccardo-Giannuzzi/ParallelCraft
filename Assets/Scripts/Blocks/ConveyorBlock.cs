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

    /// <summary>
    /// Initializes face types and populates the input faces collection.
    /// </summary>
    private void Awake()
    {
        frontFace.faceType = FaceType.Output;

        backFace.faceType = FaceType.Input;

        leftFace.faceType = FaceType.Input;

        rightFace.faceType = FaceType.Input;

        inputFaces = new IOFace[] {leftFace, backFace, rightFace};
    }

    protected override void Update()
    {
        base.Update();

        UpdateVisuals();
    }

    /// <summary>
    /// Determines whether the conveyor can start processing a new item from any available input using round robin schedule.
    /// </summary>
    /// <returns>True: if the output face is empty and an input face has a pending item; otherwise, false.</returns>
    protected override bool CanProcess()
    {
        if (frontFace.HasItem)
            return false;

        //fairly cicles across all the input faces ensuring every face item gets processed
        for (int i = 0; i < inputFaces.Length; i++)
        {
            int index = (nextInputIndex + i) % inputFaces.Length;
            IOFace face = inputFaces[index];

            if (face.HasItem)
            {
                processingFace = face;
                nextInputIndex = (index + 1) % inputFaces.Length;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Completes the process cycle by transferring the processed item from the chosen input face to the front output face.
    /// </summary>
    protected override void CompleteProcess()
    {
        if (processingFace == null)
            return;

        frontFace.currentItem = processingFace.currentItem;
        processingFace.currentItem = null;
        processingFace = null;
    }

    /// <summary>
    /// Attempts to start the item processing cycle and trigger its animation coroutine if another item is not being processed already.
    /// </summary>
    protected override void TryStartProcess()
    {
        if (isProcessing)
            return;

        if (!CanProcess())
            return;

        isProcessing = true;
        processTimer = processDelay;
        StartCoroutine(AnimateProcessing(processingFace));
    }

    /// <summary>
    /// Attempts to push the current item into the next connected node if the output path is unobstructed and ready.
    /// </summary>
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

    /// <summary>
    /// Synchronizes visual components across all matching block data faces.
    /// </summary>
    private void UpdateVisuals()
    {
        UpdateVisual(backItem, backFace);
        UpdateVisual(leftItem, leftFace);
        UpdateVisual(rightItem, rightFace);
        UpdateVisual(centerItem, frontFace);
    }

    /// <summary>
    /// Binds an item data reference into its matching world space graphic element.
    /// </summary>
    /// <param name="visual">The target display object to manipulate.</param>
    /// <param name="face">The target structural node supplying data parameters.</param>
    private void UpdateVisual(ItemVisual visual, IOFace face)
    {
        visual.SetItem(face.currentItem);
    }

    /// <summary>
    /// Runs the translation sequence sliding the item from its slot path node up to the block center.
    /// </summary>
    /// <param name="inputFace">The incoming face data reference triggering execution.</param>
    /// <returns>An IEnumerator yielding timing intervals across frames.</returns>
    private IEnumerator AnimateProcessing(IOFace inputFace)
    {
        ItemVisual visual = GetInputVisual(inputFace);
        Vector3 start = visual.transform.position;

        yield return MoveVisual(visual.transform, start, centerPosition.position, processDelay);
        visual.transform.position = start;
    }

    /// <summary>
    /// Runs the translation sequence pushing the center layout object off into the front exit node.
    /// </summary>
    /// <returns>An IEnumerator yielding timing intervals across frames.</returns>
    private IEnumerator AnimatePush()
    {
        isPushing = true;

        Vector3 start = centerItem.transform.position;
        yield return MoveVisual(centerItem.transform, start, frontPosition.position, pushDelay);
        centerItem.transform.position = start;

        TryPushFace(frontFace);
        pushTimer = pushDelay;

        isPushing = false;
    }

    /// <summary>
    /// Identifies the graphical component mapping directly over a designated raw directional system face.
    /// </summary>
    /// <param name="face">The layout reference target input check.</param>
    /// <returns>The bound <see cref="ItemVisual"/> class instance object, or null if no mapping matches.</returns>
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

    /// <summary>
    /// Translates a transform target object coordinates smoothly between vectors via a standard linear interpolation loop over time.
    /// </summary>
    /// <param name="visual">The item scene transform target requiring repositioning steps.</param>
    /// <param name="start">The source tracking vector anchor origin.</param>
    /// <param name="end">The target tracking destination point vector boundary.</param>
    /// <param name="duration">The total window execution time allotted in elapsed seconds.</param>
    /// <returns>An IEnumerator yielding execution slices iteratively per frame cycle.</returns>
    private IEnumerator MoveVisual(Transform visual, Vector3 start, Vector3 end, float duration)
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
