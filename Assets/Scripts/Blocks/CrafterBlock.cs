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
        {
            state = CrafterState.Blocked;
        }
        else
        {
            state = CrafterState.Idle;
        }

        return false;
    }

    protected override void CompleteProcess()
    {
        foreach (IOFace face in GetInputFaces())
        {
            face.currentItem = null;
        }

        GetOutputFace().currentItem = activeRecipe.outputItem;

        state = CrafterState.Idle;
    }

    private Recipe FindMatchingRecipe()
    {
        foreach (Recipe recipe in recipes)
        {
            if (RecipeMatches(recipe))
            {
                return recipe;
            }
        }

        return null;
    }

    private bool RecipeMatches(Recipe recipe)
    {
        List<Item> availableItems =
            new List<Item>();

        foreach (IOFace face in GetInputFaces())
        {
            if (face.HasItem)
            {
                availableItems.Add(face.currentItem);
            }
        }

        foreach (Item requiredItem in recipe.inputItems)
        {
            if (availableItems.Contains(requiredItem))
            {
                availableItems.Remove(requiredItem);
            }
            else
            {
                return false;
            }
        }

        return true;
    }

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