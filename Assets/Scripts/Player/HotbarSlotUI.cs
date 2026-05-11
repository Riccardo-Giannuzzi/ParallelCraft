using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private GameObject selectionBorder;

    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
        iconImage.enabled = sprite != null;
    }

    public void SetSelected(bool selected)
    {
        selectionBorder.SetActive(selected);
    }
}