using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpForce = 5.0f;

    private CharacterController charController;
    private float verticalVelocity; // Track up/down speed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
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
            // A slight push down keeps the character firmly on the ground
            verticalVelocity = gravity * Time.deltaTime;

            // Check if the player pressed the Spacebar
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // If in the air, gradually pull them down with gravity
            verticalVelocity += gravity * Time.deltaTime;
        }

        movement.y = verticalVelocity;
        movement *= Time.deltaTime;
        charController.Move(movement);
    }
}
