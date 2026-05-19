using UnityEngine;


/// <summary>
/// Handles mouse look functionality for the player.
/// </summary>
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
    private float verticalRot = 0; 
    private float horizontalRot = 0; 

    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
            body.freezeRotation = true;
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
                verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);
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