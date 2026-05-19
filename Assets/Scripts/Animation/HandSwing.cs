using UnityEngine;
using System.Collections;


/// <summary>
/// A simple script to swing a hand up and down when the left mouse button is clicked.
/// </summary>
public class HandSwing : MonoBehaviour
{
    public float swingAngle = 60f;
    public float swingSpeed = 8f;

    private Quaternion startRotation;
    private Vector3 startPosition;
    private bool isSwinging;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
            StartCoroutine(Swing());
    }

    /// <summary>
    /// Coroutine to swing the hand up and down smoothly using Slerp.
    /// </summary>
    IEnumerator Swing()
    {
        isSwinging = true;
        Quaternion targetRotation = startRotation * Quaternion.Euler(swingAngle, 0, 0);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * swingSpeed;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * swingSpeed;
            transform.localRotation = Quaternion.Slerp(targetRotation, startRotation, t);
            yield return null;
        }

        transform.localRotation = startRotation;
        isSwinging = false;
    }
}