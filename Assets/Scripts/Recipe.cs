using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipes/Recipe")]
public class Recipe : ScriptableObject
{
    public List<Item> inputItems;

    public Item outputItem;

    public float processingTime = 1f;
}