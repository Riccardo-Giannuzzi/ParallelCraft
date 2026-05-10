using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum MouseRotation
    {
        HorizontalRotation,
        VerticalRotation,
        BothRotation
    }

    public MouseRotation mouseRotation = MouseRotation.BothRotation;

    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    public float minimumVert = -90.0f;
    public float maximumVert = 90.0f;
    private float verticalRot = 0; // vertical rotation angle
    private float horizontalRot = 0; // horizontal rotation angle

    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        // This component may not have been added, so check if it exists
        if (body != null)
        {
            body.freezeRotation = true;
        }
    }

    void Update()
    {
        switch (mouseRotation)
        {
            case MouseRotation.HorizontalRotation:
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
                break;
            case MouseRotation.VerticalRotation:
                verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
                // Clamp the vertical angle between minimum and maximum limits
                verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);
                // Create a new vector from the stored rotation values.
                transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
                break;
            case MouseRotation.BothRotation:
                verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
                verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);
                horizontalRot += Input.GetAxis("Mouse X") * sensitivityHor;
                transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
                break;
            default:
                throw new System.ArgumentException("Mouse rotation wrongly specified");
        }
    }
}