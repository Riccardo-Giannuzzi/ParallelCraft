using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the list of item goals displayed in the goal UI.
/// </summary>
public class ItemGoalList : MonoBehaviour
{
    [SerializeField]
    private ItemGoalUI goalPrefab;

    private List<ItemGoalUI> activeGoals = new List<ItemGoalUI>();

    /// <summary>
    /// Builds the list of item goals in the UI based on the provided stage's goals.
    /// </summary>
    /// <param name="stage">The stage containing the goals to display.</param>
    public void BuildGoals(Stage stage)
    {
        ClearGoals();

        foreach (ItemGoal goal in stage.goals)
        {
            ItemGoalUI entry = Instantiate(goalPrefab, transform);
            entry.SetGoal(goal.item, 0, goal.requiredAmount);
            activeGoals.Add(entry);
        }
    }

    /// <summary>
    /// Updates a specific item goal in the UI with the current progress.
    /// </summary>
    /// <param name="index">The index of the goal to update.</param>
    /// <param name="current">The current progress amount.</param>
    /// <param name="target">The target amount needed to complete the goal.</param>
    /// <param name="item">The item associated with this goal.</param>
    public void UpdateGoal(int index, int current, int target, Item item)
    {
        activeGoals[index].SetGoal(item, current, target);
    }

    /// <summary>
    /// Clears all active item goals from the UI
    /// </summary>
    private void ClearGoals()
    {
        foreach (ItemGoalUI goal in activeGoals)
            Destroy(goal.gameObject);

        activeGoals.Clear();
    }
}