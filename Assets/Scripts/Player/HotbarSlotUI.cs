using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Represents the UI element for a single hotbar slot, displaying the item icon and selection highlight.
/// </summary>
public class HotbarSlotUI : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private GameObject selectionBorder;

    /// <summary>
    /// Sets the icon for this hotbar slot. If the provided sprite is null, the icon will be hidden.
    /// </summary>
    /// <param name="sprite">The sprite to display, or null to hide the icon.</param>
    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
        iconImage.enabled = sprite != null;
    }

    /// <summary>
    /// Sets whether this hotbar slot is currently selected, showing or hiding the selection border accordingly.
    /// </summary>
    /// <param name="selected">True to show the selection border, false to hide it.</param>
    public void SetSelected(bool selected)
    {
        selectionBorder.SetActive(selected);
    }
}