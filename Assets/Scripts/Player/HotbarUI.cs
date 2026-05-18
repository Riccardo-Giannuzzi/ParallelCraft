using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory inventory;

    [SerializeField]
    private HotbarSlotUI[] slotUIs = new HotbarSlotUI[9];

    void Start()
    {
        RefreshHotbar();
    }

    void Update()
    {
        UpdateSelection();
    }

    public void RefreshHotbar()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            HotbarSlot slot =
                inventory.GetItem(i);

            if (slot != null && slot.unlocked)
            {
                slotUIs[i].SetIcon(slot.icon);
            }
            else
            {
                slotUIs[i].SetIcon(null);
            }
        }
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            bool isSelected = i == inventory.SelectedSlot;

            slotUIs[i].SetSelected(isSelected);
        }
    }
}