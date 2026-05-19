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

    /// <summary>
    /// Triggers the button press animation and starts the production process in the stage manager.
    /// </summary>
    public void Interact()
    {
        if (isPressed)
            return;

        StartCoroutine(PressAnimation());
    }

    /// <summary>
    /// Animates the button press.
    /// </summary>
    private IEnumerator PressAnimation()
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

        stageManager.StartProduction();

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
}