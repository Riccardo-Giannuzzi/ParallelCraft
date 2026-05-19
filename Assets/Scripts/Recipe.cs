using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipes/Recipe")]

/// <summary>
/// Represents a crafting recipe, defining the input items required and the output item produced, as well as the time it takes to process the recipe. 
/// </summary>
public class Recipe : ScriptableObject
{
    public List<Item> inputItems;

    public Item outputItem;

    public float processingTime = 1f;
}