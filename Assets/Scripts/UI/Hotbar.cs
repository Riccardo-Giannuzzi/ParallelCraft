using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private int selected;
    private int minSelected = 1;
    private int maxSelected = 10;

    public PlayerMovement playerReference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selected = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Select(int selected)
    {
        this.selected = selected;

    }
}
