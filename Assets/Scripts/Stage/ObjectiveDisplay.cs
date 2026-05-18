using TMPro;
using UnityEngine;

public class ObjectiveDisplay : MonoBehaviour
{
    [SerializeField]
    private Transform goalsParent;

    [SerializeField]
    private ItemGoalDisplay goalPrefab;

    [SerializeField]
    private Image recipeImage;

    public void SetObjective(string text)
    {
        objectiveText.text = text;
    }

    public void SetTimer(float timeLeft)
    {
        timerText.text =
            Mathf.CeilToInt(timeLeft).ToString();
    }
}