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

    private void RefreshHotbar()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            ToolData item = inventory.GetItem(i);

            if (item != null)
            {
                slotUIs[i].SetIcon(item.icon);
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