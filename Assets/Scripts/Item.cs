using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]

/// <summary>
/// Represents a generic item in the game.
/// </summary>
public class Item : ScriptableObject
{
    public string itemName;

    public Sprite icon;
}