using UnityEngine;

public enum FaceType
{
    Closed,
    Input,
    Output
}

public class IOFace : MonoBehaviour
{
    public FaceType faceType;

    public IOFace connectedFace;

    public bool IsConnected => connectedFace != null;

    public bool CanConnectTo(IOFace other)
    {
        if (other == null)
            return false;

        if (IsConnected || other.IsConnected)
            return false;

        if (faceType == FaceType.Input &&
            other.faceType == FaceType.Output)
        {
            return true;
        }

        if (faceType == FaceType.Output &&
            other.faceType == FaceType.Input)
        {
            return true;
        }

        return false;
    }

    public bool Connect(IOFace other)
    {
        if (!CanConnectTo(other))
            return false;

        if (IsConnected || other.IsConnected)
            return false;

        connectedFace = other;
        other.connectedFace = this;

        return true;
    }

    public void Disconnect()
    {
        if (connectedFace != null)
        {
            connectedFace.connectedFace = null;
            connectedFace = null;
        }
    }

    private void OnDrawGizmos()
    {
        switch (faceType)
        {
            case FaceType.Input:
                Gizmos.color = Color.blue;
                break;

            case FaceType.Output:
                Gizmos.color = Color.red;
                break;

            default:
                Gizmos.color = Color.gray;
                break;
        }

        Gizmos.DrawSphere(transform.position, 0.1f);

        if (connectedFace != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(
                transform.position,
                connectedFace.transform.position
            );
        }
    }
}