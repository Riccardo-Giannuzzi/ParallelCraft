using UnityEngine;

public class ItemVisual : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SetItem(Item item)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        spriteRenderer.enabled = true;

        spriteRenderer.sprite = item.icon;
    }

    public void Clear()
    {
        spriteRenderer.sprite = null;

        spriteRenderer.enabled = false;
    }
}