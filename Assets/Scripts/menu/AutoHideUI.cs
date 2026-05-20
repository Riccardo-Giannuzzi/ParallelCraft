using System.Collections;
using UnityEngine;

/// <summary>
/// Automatically hides the UI panel after a certain amount of time, with a smooth sliding animation.
/// </summary>
public class AutoHideUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private float visibleTime = 5f;

    [SerializeField]
    private float moveDistance = 200f;

    [SerializeField]
    private float animationTime = 1f;

    private void Start()
    {
        StartCoroutine(HideRoutine());
    }

    /// <summary>
    ///  Coroutine that handles the timing and animation for hiding the UI panel. 
    /// </summary>
    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(visibleTime);

        Vector2 start = panel.anchoredPosition;
        Vector2 end = start + Vector2.down * moveDistance;
        float timer = 0f;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            float t = timer / animationTime;
            panel.anchoredPosition = Vector2.Lerp(start, end, t);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}