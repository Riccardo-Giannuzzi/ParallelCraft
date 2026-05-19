using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum StagePhase
{
    Build,
    Production,
    Success,
    Failure,
    Completed
}

public class StageManager : MonoBehaviour
{
    [Header("Stages")]
    [SerializeField] private List<Stage> stages;

    private int currentStageIndex;
    private Stage currentStage;
    private List<SourceBlock> sourceBlocks = new List<SourceBlock>();

    [Header("References")]
    [SerializeField] private SinkBlock sinkBlock;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private ObjectiveDisplay objectiveDisplay;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private HotbarUI hotbarUI;

    [Header("Messages")]
    [SerializeField]
    private TMP_Text messageText;

    [SerializeField]
    private float messageDuration = 2f;


    private StagePhase phase;
    
    private float timer;

    private void Start()
    {
        InitializeSources();

        LoadStage(0);
    }

    private void Update()
    {
        if (phase != StagePhase.Production)
            return;

        timer -= Time.deltaTime;

        objectiveDisplay.SetTime(timer);

        UpdateGoals();

        if (CheckStageCompleted())
        {
            CompleteStage();
        }

        if (timer <= 0f)
        {
            FailStage();
        }
    }

    private void LoadStage(int index)
    {
        currentStageIndex = index;
        currentStage = stages[index];

        phase = StagePhase.Build;
        timer = currentStage.timeLimit;

        objectiveDisplay.ShowStage(currentStage);

        playerInventory.UpdateUnlockedSlots(currentStageIndex);

        hotbarUI.RefreshHotbar();

        UpdateUnlockedSources();

        DisableSources();

        sinkBlock.ResetCounts();
    }

    public void StartProduction()
    {
        if (phase != StagePhase.Build)
            return;

        phase = StagePhase.Production;

        playerInventory.LockAllSlots();

        hotbarUI.RefreshHotbar();

        EnableSources();
    }

    private void InitializeSources()
    {
        foreach (IOBlock block in
            placementSystem.GetAllIOBlocks())
        {
            SourceBlock source =
                block as SourceBlock;

            if (source == null)
                continue;

            source.Lock();

            sourceBlocks.Add(source);
        }
    }
    private void EnableSources()
    {
        foreach (SourceBlock source in sourceBlocks)
        {
            if (source.UnlockStage <=
                currentStageIndex)
            {
                source.Activate();
            }
        }
    }

    private void UpdateUnlockedSources()
    {
        foreach (SourceBlock source in sourceBlocks)
        {
            if (source.UnlockStage <= currentStageIndex)
            {
                source.Unlock();
            }
            else
            {
                source.Lock();
            }
        }
    }

    private void DisableSources()
    {
        foreach (SourceBlock source in sourceBlocks)
        {
            source.Deactivate();
        }
    }

    private void UpdateGoals()
    {
        for (int i = 0; i < currentStage.goals.Count; i++)
        {
            ItemGoal goal = currentStage.goals[i];

            int currentAmount = sinkBlock.GetItemCount(goal.item);

            objectiveDisplay.UpdateGoal(
                i,
                goal.item,
                currentAmount,
                goal.requiredAmount
            );
        }
    }

    private bool CheckStageCompleted()
    {
        foreach (ItemGoal goal in currentStage.goals)
        {
            int currentAmount = sinkBlock.GetItemCount(goal.item);

            if (currentAmount < goal.requiredAmount)
            {
                return false;
            }
        }

        return true;
    }

    private void CompleteStage()
    {
        StartCoroutine(HandleStageComplete());
    }

    private void FailStage()
    {
        StartCoroutine(HandleStageFail());
    }

    private void LevelCompleted()
    {
        phase = StagePhase.Completed;
    }

    private void ClearAllBlocks()
    {
        foreach (IOBlock block in
            placementSystem.GetAllIOBlocks())
        {
            block.ClearItems();
        }
    }

    private IEnumerator FlashMessageCoroutine(string message)
    {
        messageText.gameObject.SetActive(true);

        messageText.text = message;

        yield return new WaitForSeconds(
            messageDuration
        );

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator HandleStageComplete()
    {
        phase = StagePhase.Success;

        DisableSources();

        ClearAllBlocks();

        sinkBlock.ResetCounts();

        int nextStage =
            currentStageIndex + 1;

        if (nextStage >= stages.Count)
        {
            yield return FlashMessageCoroutine(
                "LEVEL COMPLETED"
            );

            LevelCompleted();

            yield break;
        }

        yield return FlashMessageCoroutine(
            "STAGE COMPLETED"
        );

        LoadStage(nextStage);
    }

    private IEnumerator HandleStageFail()
    {
        phase = StagePhase.Failure;

        DisableSources();

        ClearAllBlocks();

        sinkBlock.ResetCounts();

        yield return FlashMessageCoroutine(
            "STAGE FAILED"
        );

        phase = StagePhase.Build;

        timer = currentStage.timeLimit;

        objectiveDisplay.ShowStage(currentStage);

        UpdateGoals();
    }
}