using UnityEngine;

public enum ItemType
{
    Tool,
    Placeable
}

[System.Serializable]
public class ToolData
{
    public string itemName;

    public ItemType itemType;

    public GameObject handObject;

    public GameObject placeablePrefab;

    public Sprite icon;
}