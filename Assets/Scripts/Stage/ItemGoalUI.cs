using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component for displaying an individual item goal in the stage objective display.
/// </summary>
public class ItemGoalUI : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TMP_Text countText;

    /// <summary>
    /// Sets the item goal UI to display the specified item and progress towards the goal.
    /// </summary>
    /// <param name="item">The item to display in the goal UI.</param>
    /// <param name="current">The current amount of the item collected.</param>
    /// <param name="required">The required amount of the item to complete the goal.</param>
    public void SetGoal(Item item, int current, int required)
    {
        itemIcon.sprite = item.icon;
        countText.text = current + "/" + required;
    }
}