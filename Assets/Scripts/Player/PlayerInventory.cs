using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Slots")]
    [SerializeField]
    private ItemData[] slots;

    public int SelectedSlot { get; private set; } = 0;

    void Start()
    {
        DeactivateAllSlots();
        ActivateSlot(0);
    }

    void Update()
    {
        HandleNumberKeys();
        HandleMouseScroll();
    }

    private void HandleNumberKeys()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            KeyCode key = KeyCode.Alpha1 + i;

            if (Input.GetKeyDown(key))
            {
                ActivateSlot(i);
            }
        }
    }

    private void HandleMouseScroll()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0f)
        {
            ScrollUp();
        }
        else if (scroll < 0f)
        {
            ScrollDown();
        }
    }

    private void ScrollUp()
    {
        int nextSlot = SelectedSlot + 1;

        if (nextSlot >= slots.Length)
        {
            nextSlot = 0;
        }

        ActivateSlot(nextSlot);
    }

    private void ScrollDown()
    {
        int nextSlot = SelectedSlot - 1;

        if (nextSlot < 0)
        {
            nextSlot = slots.Length - 1;
        }

        ActivateSlot(nextSlot);
    }

    private void DeactivateAllSlots()
    {
        foreach (ItemData item in slots)
        {
            if (item != null && item.handObject != null)
            {
                item.handObject.SetActive(false);
            }
        }
    }

    private void ActivateSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
            return;

        DeactivateAllSlots();

        SelectedSlot = slotIndex;

        ItemData selectedItem = slots[slotIndex];

        if (selectedItem != null && selectedItem.handObject != null)
        {
            selectedItem.handObject.SetActive(true);
        }
    }

    public ItemData GetCurrentItem()
    {
        if (SelectedSlot >= 0 && SelectedSlot < slots.Length)
        {
            return slots[SelectedSlot];
        }

        return null;
    }

    public ItemData GetItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            return slots[index];
        }

        return null;
    }
}