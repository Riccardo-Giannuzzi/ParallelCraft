using System.Collections.Generic;
using UnityEngine;

public class ItemGoalList : MonoBehaviour
{
    [SerializeField]
    private ItemGoalDisplay goalPrefab;

    private List<ItemGoalDisplay> activeGoals =
        new List<ItemGoalDisplay>();

    public void BuildGoals(Stage stage)
    {
        ClearGoals();

        foreach (ItemGoal goal in stage.goals)
        {
            ItemGoalDisplay entry =
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
        foreach (ItemGoalDisplay goal in activeGoals)
        {
            Destroy(goal.gameObject);
        }

        activeGoals.Clear();
    }
}