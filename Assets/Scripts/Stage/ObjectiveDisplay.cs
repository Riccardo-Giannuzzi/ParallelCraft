using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void ShowStage(Stage stage)
    {
        stageTitleText.text = stage.title;

        recipeDiagramImage.sprite = stage.recipeDiagram;

        BuildGoals(stage);
    }

    private void BuildGoals(Stage stage)
    {
        ClearGoals();

        foreach (ItemGoal goal in stage.goals)
        {
            ItemGoalUI goalUI =
                Instantiate(
                    goalPrefab,
                    itemGoalList
                );

            goalUI.SetGoal(
                goal.item,
                0,
                goal.requiredAmount
            );

            goalUIs.Add(goalUI);
        }
    }

    public void UpdateGoal(
        int index,
        Item item,
        int current,
        int target)
    {
        if (index < 0 ||
            index >= goalUIs.Count)
        {
            return;
        }

        goalUIs[index].SetGoal(
            item,
            current,
            target
        );
    }

    public void SetTime(float timeLeft)
    {
        int seconds =
            Mathf.CeilToInt(timeLeft);

        timeText.text =
            seconds.ToString();
    }

    private void ClearGoals()
    {
        foreach (ItemGoalUI goalUI in goalUIs)
        {
            Destroy(goalUI.gameObject);
        }

        goalUIs.Clear();
    }
}