using System.Collections;
using UnityEngine;


/// <summary>
/// Represents a button in the game world that the player can interact with to trigger an action.
/// </summary>
public class Button : MonoBehaviour, IInteractable
{
    [SerializeField]
    private StageManager stageManager;

    [SerializeField]
    private Transform buttonVisual;

    [SerializeField]
    private float pressDistance = 0.1f;

    [SerializeField]
    private float animationTime = 0.1f;

    private bool isPressed;

    [SerializeField]
    private MeshRenderer buttonRenderer;

    [SerializeField]
    private Material readyMaterial;

    [SerializeField]
    private Material runningMaterial;

    /// <summary>
    /// Triggers the button press animation and starts the production process in the stage manager.
    /// </summary>
    public void Interact()
    {
        if (isPressed)
            return;

        if (stageManager.CurrentPhase == StagePhase.Build)
        {
            StartCoroutine(
                PressAnimation(true)
            );
        }
        else if (stageManager.CurrentPhase == StagePhase.Production)
        {
            StartCoroutine(
                PressAnimation(false)
            );
        }
    }

    /// <summary>
    /// Animates the button press. if startProduction is true, the stage manager will start production after the button is pressed; otherwise, it will abort production.
    /// The button will return to its original position after the animation is complete.
    /// </summary>
    /// <param name="startProduction">Whether to start production after the button press.</param>
    private IEnumerator PressAnimation(bool startProduction)
    {
        isPressed = true;

        Vector3 start = buttonVisual.localPosition;
        Vector3 pressed = start + Vector3.down * pressDistance;

        float timer = 0f;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            float t = timer / animationTime;
            buttonVisual.localPosition = Vector3.Lerp(start, pressed, t);
            yield return null;
        }

        if (startProduction)
            stageManager.StartProduction();
        else
            stageManager.AbortProduction();

        timer = 0f;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            float t = timer / animationTime;
            buttonVisual.localPosition = Vector3.Lerp(pressed, start, t);
            yield return null;
        }

        buttonVisual.localPosition = start;
        isPressed = false;
    }

    private StagePhase lastPhase;

    /// <summary>
    /// Updates the button's material based on the current phase of the stage. If the stage is in the production phase, the button will use the running material; otherwise, it will use the ready material.
    /// </summary>
    private void Update()
    {
        if (lastPhase == stageManager.CurrentPhase)
            return;

        lastPhase = stageManager.CurrentPhase;

        if (lastPhase == StagePhase.Production)
            buttonRenderer.material = runningMaterial;
        else
            buttonRenderer.material = readyMaterial;
    }
}