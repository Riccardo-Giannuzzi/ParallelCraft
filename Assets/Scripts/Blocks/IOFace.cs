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

    public Item currentItem;

    public bool HasItem => currentItem != null;

    /// <summary>
    /// Verifies if the face has any item
    /// </summary>
    /// <returns>True: has item, False: doesn't have item</returns>
    public bool CanReceiveItem()
    {
        return !HasItem;
    }

    public bool IsConnected => connectedFace != null;

    /// <summary>
    /// Verifies if the given face can connect logically to this one (only Input and Output faces are compatible)
    /// </summary>
    /// <param name="other">The other face</param>
    /// <returns>True: can connect, False: can't connect</returns>
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


    /// <summary>
    /// Connects, if possible, the argument face to the current one
    /// </summary>
    /// <param name="other">The other face</param>
    /// <returns>True: can connect, False: can't connect</returns>
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

    /// <summary>
    /// Disconnects The current face from the one it's connected to
    /// </summary>
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
            Gizmos.DrawLine(transform.position, connectedFace.transform.position);
        }
    }
}