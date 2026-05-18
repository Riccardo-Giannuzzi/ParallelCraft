using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGoalDisplay : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TMP_Text countText;

    public void SetGoal(
        Item item,
        int current,
        int required)
    {
        itemIcon.sprite = item.icon;

        countText.text = current + "/" + required;
    }
}