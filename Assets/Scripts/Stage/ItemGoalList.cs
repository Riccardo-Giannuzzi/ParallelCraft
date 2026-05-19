using System.Collections.Generic;
using UnityEngine;

public class ItemGoalList : MonoBehaviour
{
    [SerializeField]
    private ItemGoalUI goalPrefab;

    private List<ItemGoalUI> activeGoals =
        new List<ItemGoalUI>();

    public void BuildGoals(Stage stage)
    {
        ClearGoals();

        foreach (ItemGoal goal in stage.goals)
        {
            ItemGoalUI entry =
                Instantiate(
                    goalPrefab,
                    transform
                );

            entry.SetGoal(
                goal.item,
                0,
                goal.requiredAmount
            );

            activeGoals.Add(entry);
        }
    }

    public void UpdateGoal(
        int index,
        int current,
        int target,
        Item item)
    {
        activeGoals[index].SetGoal(
            item,
            current,
            target
        );
    }

    private void ClearGoals()
    {
        foreach (ItemGoalUI goal in activeGoals)
        {
            Destroy(goal.gameObject);
        }

        activeGoals.Clear();
    }
}