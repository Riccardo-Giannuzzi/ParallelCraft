using UnityEngine;

/// <summary>
/// Creeper class that rotates the game object to always face the player.
/// </summary>
public class Creeper : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float rotationSpeed = 5f;

    private void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}