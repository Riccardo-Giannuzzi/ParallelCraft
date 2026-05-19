using UnityEngine;


/// <summary>
/// Handles player movement using.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpForce = 5.0f;

    private CharacterController charController;
    private float verticalVelocity;

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        // We transform the X and Z direction FIRST, before adding the jump
        movement = transform.TransformDirection(movement);

        // Jumping and Gravity logic
        if (charController.isGrounded)
        {
            verticalVelocity = gravity;

            if (Input.GetButtonDown("Jump"))
                verticalVelocity = jumpForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        movement.y = verticalVelocity;
        movement *= Time.deltaTime;
        charController.Move(movement);
    }
}
