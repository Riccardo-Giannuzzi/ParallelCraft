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
}