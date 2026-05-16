using UnityEngine;

public class ConveyorAnimator : MonoBehaviour
{
    public Texture2D[] frames;
    public Material conveyorMaterial;
    [SerializeField]
    public float fps;

    void Update()
    {
        int frame = (int)(Time.time * fps) % frames.Length;
        conveyorMaterial.mainTexture = frames[frame];
    }
}