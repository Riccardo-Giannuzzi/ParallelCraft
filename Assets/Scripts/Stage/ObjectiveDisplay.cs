using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the display of stage objectives, including the stage title, time limit, item goals, and recipe diagram.
/// </summary>
public class ObjectiveDisplay : MonoBehaviour
{

    [SerializeField]
    private TMP_Text stageTitleText;

    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private Transform itemGoalList;

    [SerializeField]
    private ItemGoalUI goalPrefab;

    private List<ItemGoalUI> goalUIs = new List<ItemGoalUI>();

    [SerializeField]
    private Image recipeDiagramImage;

    /// <summary>
    /// Sets up the objective display with the information from the given stage, including the stage title, time limit, recipe diagram, and item goals.
    /// The item goals are built based on the stage's goals and displayed in the UI.
    /// </summary>
    /// <param name="stage">The stage whose objectives are to be displayed.</param>
    /// <returns></returns>
    public void ShowStage(Stage stage)
    {
        stageTitleText.text = stage.title;
        SetTime(stage.timeLimit);
        recipeDiagramImage.sprite = stage.recipeDiagram;
        BuildGoals(stage);
    }

    /// <summary>
    /// Hides the objective display by clearing the stage title, time text, recipe diagram, and item goals from the UI.
    /// </summary>
    /// <param name="stage">The stage whose objectives are to be hidden.</param>
    private void BuildGoals(Stage stage)
    {
        ClearGoals();

        foreach (ItemGoal goal in stage.goals)
        {
            ItemGoalUI goalUI = Instantiate(goalPrefab, itemGoalList);
            goalUI.SetGoal(goal.item, 0, goal.requiredAmount);
            goalUIs.Add(goalUI);
        }
    }


    /// <summary>
    /// Updates a specific item goal in the UI with the current progress.
    /// </summary>
    /// <param name="index">The index of the goal to update.</param>
    /// <param name="item">The item associated with this goal.</param>
    /// <param name="current">The current progress amount.</param>
    /// <param name="target">The target amount needed to complete the goal.</param>
    public void UpdateGoal(int index, Item item, int current, int target)
    {
        if (index < 0 || index >= goalUIs.Count)
            return;

        goalUIs[index].SetGoal(item, current, target);
    }

    /// <summary>
    /// Sets the time display text showing the remaining time for the stage.
    /// </summary>
    /// <param name="timeLeft">The time remaining in seconds.</param>
    public void SetTime(float timeLeft)
    {
        int totalSeconds = Mathf.CeilToInt(timeLeft);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timeText.text = minutes.ToString() + ":" + seconds.ToString("00");
    }

    /// <summary>
    /// Hides the objective display by clearing the stage title, time text, recipe diagram, and item goals from the UI.
    /// </summary>
    private void ClearGoals()
    {
        foreach (ItemGoalUI goalUI in goalUIs)
            Destroy(goalUI.gameObject);

        goalUIs.Clear();
    }
}