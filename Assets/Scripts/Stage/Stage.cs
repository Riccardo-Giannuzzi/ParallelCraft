using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stages/Stage")]
public class Stage : ScriptableObject
{
    [TextArea]
    public string title;

    public float timeLimit;

    public List<ItemGoal> goals;

    public Sprite recipeDiagram;
}