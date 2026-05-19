using UnityEngine;


/// <summary>
/// Manages the hotbar UI.
/// </summary>
public class HotbarUI : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory inventory;

    [SerializeField]
    private HotbarSlotUI[] slotUIs = new HotbarSlotUI[9];

    [SerializeField]
    private Sprite lockedIcon;

    void Start()
    {
        RefreshHotbar();
    }

    void Update()
    {
        UpdateSelection();
    }

    /// <summary>
    /// Refreshes the hotbar UI to reflect the current state of the player's inventory. 
    /// </summary>
    public void RefreshHotbar()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            HotbarSlot slot = inventory.GetItem(i);

            if (slot == null)
            {
                slotUIs[i].SetIcon(null);

                continue;
            }

            if (slot.unlocked)
            {
                slotUIs[i].SetIcon(slot.icon);
            }
            else
            {
                slotUIs[i].SetIcon(lockedIcon);
            }
        }
    }

    /// <summary>
    /// Updates the selection highlight on the hotbar to match the currently selected slot in the player's inventory.
    /// </summary>
    private void UpdateSelection()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            bool isSelected = i == inventory.SelectedSlot;
            slotUIs[i].SetSelected(isSelected);
        }
    }
}