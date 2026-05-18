using UnityEngine;


/// <summary>
/// class for the handling of a visual item
/// </summary>
public class ItemVisual : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Updates the visual display to match the specified item data, enabling the renderer and applying its icon.
    /// </summary>
    /// <param name="item">The item data containing the icon sprite to display. Passing null clears the visual.</param>
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

    /// <summary>
    /// Clears the current sprite and disables the renderer to visually hide the item.
    /// </summary>
    public void Clear()
    {
        spriteRenderer.sprite = null;
        spriteRenderer.enabled = false;
    }
}