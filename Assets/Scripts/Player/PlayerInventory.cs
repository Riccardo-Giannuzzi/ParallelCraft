using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Slots")]
    [SerializeField]
    private HotbarSlot[] slots;

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
        foreach (HotbarSlot item in slots)
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

        HotbarSlot selectedItem = slots[slotIndex];

        if (selectedItem != null && selectedItem.handObject != null)
        {
            selectedItem.handObject.SetActive(true);
        }
    }

    public HotbarSlot GetCurrentSlot()
    {
        if (SelectedSlot < 0 ||
            SelectedSlot >= slots.Length)
        {
            return null;
        }

        HotbarSlot slot =
            slots[SelectedSlot];

        if (!slot.unlocked)
            return null;

        return slot;
    }

    public HotbarSlot GetItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            return slots[index];
        }

        return null;
    }

    public void UnlockSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        slots[index].unlocked = true;
    }

    public void LockAllSlots()
    {
        foreach (HotbarSlot slot in slots)
        {
            slot.unlocked = false;
        }

        DeactivateAllSlots();
    }

    public void UnlockAllSlots()
    {
        foreach (HotbarSlot slot in slots)
        {
            slot.unlocked = true;
        }
    }
}