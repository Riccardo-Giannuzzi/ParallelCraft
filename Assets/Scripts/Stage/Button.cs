using System.Collections;
using UnityEngine;

public class Button :
    MonoBehaviour,
    IInteractable
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

    public void Interact()
    {
        if (isPressed)
            return;

        StartCoroutine(PressAnimation());
    }

    private IEnumerator PressAnimation()
    {
        isPressed = true;

        Vector3 start =
            buttonVisual.localPosition;

        Vector3 pressed =
            start + Vector3.down * pressDistance;

        float timer = 0f;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;

            float t =
                timer / animationTime;

            buttonVisual.localPosition =
                Vector3.Lerp(start, pressed, t);

            yield return null;
        }

        stageManager.StartProduction();

        timer = 0f;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;

            float t =
                timer / animationTime;

            buttonVisual.localPosition =
                Vector3.Lerp(pressed, start, t);

            yield return null;
        }

        buttonVisual.localPosition = start;

        isPressed = false;
    }
}