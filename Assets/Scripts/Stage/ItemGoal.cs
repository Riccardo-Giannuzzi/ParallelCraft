using UnityEngine;

[System.Serializable]

/// <summary>
///  Represents a goal that requires the player to collect a certain amount of a specific item.
/// </summary>
public class ItemGoal
{
    public Item item;

    public int requiredAmount;
}