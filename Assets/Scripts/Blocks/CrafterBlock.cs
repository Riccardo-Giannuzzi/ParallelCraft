using System.Collections.Generic;
using UnityEngine;

public enum CrafterState
{
    Idle,
    Processing,
    Blocked
}

public class CrafterBlock : IOBlock
{
    [Header("Crafter")]
    [SerializeField]
    private List<Recipe> recipes;

    public CrafterState state;

    private Recipe activeRecipe;



    /// <summary>
    /// Determines if the crafting block can start processing by checking the output availability and recipe requirements.
    /// </summary>
    /// <returns>True if a matching recipe is found and processing can begin; otherwise, false.</returns>
    protected override bool CanProcess()
    {
        IOFace outputFace = GetOutputFace();

        if (outputFace == null)
            return false;

        if (outputFace.HasItem)
            return false;

        activeRecipe = FindMatchingRecipe();

        if (activeRecipe != null)
        {
            processDelay = activeRecipe.processingTime;
            state = CrafterState.Processing;
            return true;
        }

        if (AllInputFacesOccupied())
            state = CrafterState.Blocked;
        else
            state = CrafterState.Idle;

        return false;
    }

    /// <summary>
    /// Completes the crafting process by consuming the input ingredients and placing the crafted item on the output face.
    /// </summary>
    protected override void CompleteProcess()
    {
        foreach (IOFace face in GetInputFaces())
            face.currentItem = null;
        
        GetOutputFace().currentItem = activeRecipe.outputItem;
        state = CrafterState.Idle;
    }

    /// <summary>
    /// Searches through the available recipes to find one that matches the items currently on the input faces.
    /// </summary>
    /// <returns>The matching recipe if found; otherwise, null.</returns>
    private Recipe FindMatchingRecipe()
    {
        foreach (Recipe recipe in recipes)
        {
            if (RecipeMatches(recipe))
                return recipe;
        }

        return null;
    }

    /// <summary>
    /// Checks if the current items on the input faces satisfy the requirements of a specific recipe.
    /// </summary>
    /// <param name="recipe">The recipe to validate against the available input items.</param>
    /// <returns>True if all required ingredients are present; otherwise, false.</returns>
    private bool RecipeMatches(Recipe recipe)
    {
        List<Item> availableItems =
            new List<Item>();

        foreach (IOFace face in GetInputFaces())
        {
            if (face.HasItem)
                availableItems.Add(face.currentItem);
        }

        foreach (Item requiredItem in recipe.inputItems)
        {
            if (availableItems.Contains(requiredItem))
                availableItems.Remove(requiredItem);
            else
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks whether all available input faces currently hold an item.
    /// </summary>
    /// <returns>True if every input face is occupied; otherwise, false.</returns>
    private bool AllInputFacesOccupied()
    {
        foreach (IOFace face in GetInputFaces())
        {
            if (!face.HasItem)
                return false;
        }

        return true;
    }
}