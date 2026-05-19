using UnityEngine;

/// <summary>
/// Manages the player's hotbar inventory.
/// </summary>
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

    /// <summary>
    /// Handles input for selecting hotbar slots using number keys. 
    /// </summary>
    private void HandleNumberKeys()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            KeyCode key = KeyCode.Alpha1 + i;
            if (Input.GetKeyDown(key))
                ActivateSlot(i);
        }
    }

    /// <summary>
    /// Handles input for selecting hotbar slots using the mouse scroll wheel.
    /// </summary>
    private void HandleMouseScroll()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0f)
            ScrollUp();
        else if (scroll < 0f)
            ScrollDown();
    }

    /// <summary>
    /// Next slot selection when scrolling up.
    /// </summary>
    private void ScrollUp()
    {
        int nextSlot = SelectedSlot + 1;

        if (nextSlot >= slots.Length)
            nextSlot = 0;

        ActivateSlot(nextSlot);
    }

    /// <summary>
    /// Previous slot selection when scrolling up.
    /// </summary>
    private void ScrollDown()
    {
        int nextSlot = SelectedSlot - 1;

        if (nextSlot < 0)
        {
            nextSlot = slots.Length - 1;
        }

        ActivateSlot(nextSlot);
    }

    /// <summary>
    /// Deactivates all hotbar slots, hiding any associated hand objects.
    /// </summary>
    private void DeactivateAllSlots()
    {
        foreach (HotbarSlot item in slots)
        {
            if (item != null && item.handObject != null)
                item.handObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activates the specified hotbar slot, making its associated hand object visible if it is unlocked. 
    /// </summary>
    private void ActivateSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
            return;

        DeactivateAllSlots();

        SelectedSlot = slotIndex;
        HotbarSlot selectedItem = slots[slotIndex];

        if (selectedItem != null && selectedItem.handObject != null)
            selectedItem.handObject.SetActive(true);
    }


    /// <summary>
    /// Returns selected hotbar slot if unlocked.
    /// </summary>
    /// <returns>Selected hotbar slot</returns>
    public HotbarSlot GetCurrentSlot()
    {
        if (SelectedSlot < 0 || SelectedSlot >= slots.Length)
            return null;

        HotbarSlot slot = slots[SelectedSlot];

        if (!slot.unlocked)
            return null;

        return slot;
    }

    public void UpdateUnlockedSlots(
    int currentStage)
    {
        foreach (HotbarSlot slot in slots)
        {
            slot.unlocked =
                slot.unlockStage <= currentStage;
        }
    }


    public HotbarSlot GetItem(int index)
    {
        if (index >= 0 && index < slots.Length)
            return slots[index];
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
            slot.unlocked = false;

        DeactivateAllSlots();
    }

    public void UnlockAllSlots()
    {
        foreach (HotbarSlot slot in slots)
            slot.unlocked = true;
    }
}