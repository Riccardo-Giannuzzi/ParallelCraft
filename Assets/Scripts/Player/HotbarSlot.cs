using UnityEngine;

public enum ItemType
{
    Tool,
    Placeable,
    Empty
}

[System.Serializable]

/// <summary>
/// Represents a single slot in the player's hotbar inventory.
/// </summary>
public class HotbarSlot
{
    public string itemName;

    public ItemType itemType;

    public GameObject handObject;

    public GameObject placeablePrefab;

    public Sprite icon;

    public int unlockStage;

    // Whether the slot is unlocked and can be used by the player. Locked slots are not accessible and do not display an icon in the hotbar UI.
    public bool unlocked = true;
}