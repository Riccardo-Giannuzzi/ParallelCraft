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

/// <summary>
/// Manages the stages of the game, including loading stages, starting production, checking for stage completion, and handling stage success or failure.
/// </summary>
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
            CompleteStage();

        if (timer <= 0f)
            FailStage();
    }

    /// <summary>
    /// Loads the stage at the specified index, setting up the objective display, updating the player's inventory for unlocked slots, refreshing the hotbar UI, and updating the unlocked sources based on the current stage. It also resets the sink blocks and disables all sources to prepare for the new stage.
     ///
    /// </summary>
    /// <param name="index">The index of the stage to load.</param>
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

    /// <summary>
    /// Starts the production phase of the current stage, locking the player's inventory slots, refreshing the hotbar UI, and enabling the sources that are unlocked for the current stage. This method can only be called if the current phase is Build, and it transitions the stage into the Production phase.
    /// </summary>
    public void StartProduction()
    {
        if (phase != StagePhase.Build)
            return;

        phase = StagePhase.Production;

        playerInventory.LockAllSlots();
        hotbarUI.RefreshHotbar();
        EnableSources();
    }

    /// <summary>
    /// Finds all source blocks in the placement system, locks them, and caches them.
    /// </summary>
    private void InitializeSources()
    {
        foreach (IOBlock block in placementSystem.GetAllIOBlocks())
        {
            SourceBlock source = block as SourceBlock;

            if (source == null)
                continue;

            source.Lock();
            sourceBlocks.Add(source);
        }
    }

    /// <summary>
    /// Activates all cached sources that are unlocked for the current stage.
    /// </summary>
    private void EnableSources()
    {
        foreach (SourceBlock source in sourceBlocks)
        {
            if (source.IsActiveInStage(currentStageIndex))
            {
                source.Activate();
            }
        }
    }

    /// <summary>
    /// Unlocks or locks cached sources based on the current stage.
    /// </summary>
    private void UpdateUnlockedSources()
    {
        foreach (SourceBlock source in sourceBlocks)
        {
            if (source.IsActiveInStage(currentStageIndex))
            {
                source.Unlock();
            }
            else
            {
                source.Lock();
            }
        }
    }

    /// <summary>
    /// Deactivates all cached source blocks.
    /// </summary>
    private void DisableSources()
    {
        foreach (SourceBlock source in sourceBlocks)
            source.Deactivate();
    }

    /// <summary>
    /// Updates the objective display with current goal progress.
    /// </summary>
    private void UpdateGoals()
    {
        for (int i = 0; i < currentStage.goals.Count; i++)
        {
            ItemGoal goal = currentStage.goals[i];
            int currentAmount = sinkBlock.GetItemCount(goal.item);
            objectiveDisplay.UpdateGoal(i, goal.item, currentAmount, goal.requiredAmount);
        }
    }

    /// <summary>
    /// Returns true if all item goals have been met.
    /// </summary>
    /// <returns>True if all item goals are completed; false otherwise.</returns>
    private bool CheckStageCompleted()
    {
        foreach (ItemGoal goal in currentStage.goals)
        {
            int currentAmount = sinkBlock.GetItemCount(goal.item);            
            if (currentAmount < goal.requiredAmount)
                return false;
        }

        return true;
    }

   
    /// <summary>
    /// Triggers completion handling for the current stage.
    /// </summary>
    private void CompleteStage()
    {
        StartCoroutine(HandleStageComplete());
    }

    /// <summary>
    /// Triggers failure handling for the current stage.
    /// </summary>
    private void FailStage()
    {
        StartCoroutine(HandleStageFail());
    }

    /// <summary>
    /// Marks the level as fully completed.
    /// </summary>
    private void LevelCompleted()
    {
        phase = StagePhase.Completed;
    }

    /// <summary>
    /// Clears items from all placed IO blocks.
    /// </summary>
    private void ClearAllBlocks()
    {
        foreach (IOBlock block in placementSystem.GetAllIOBlocks())
            block.ClearItems();
    }

    /// <summary>
    /// Aborts production and triggers failure flow.
    /// </summary>
    public void AbortProduction()
    {
        if (phase != StagePhase.Production)
            return;

        FailStage();
    }

    /// <summary>
    /// Shows a temporary message on screen for a short duration.
    /// </summary>
    /// <param name="message">The message to display.</param>
    private IEnumerator FlashMessageCoroutine(string message)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        yield return new WaitForSeconds(messageDuration);

        messageText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine handling successful stage completion flow.
    /// </summary>
    private IEnumerator HandleStageComplete()
    {
        phase = StagePhase.Success;

        DisableSources();
        ClearAllBlocks();
        sinkBlock.ResetCounts();

        int nextStage = currentStageIndex + 1;

        if (nextStage >= stages.Count)
        {
            yield return FlashMessageCoroutine("LEVEL COMPLETED");
            LevelCompleted();
            yield break;
        }

        yield return FlashMessageCoroutine("STAGE COMPLETED");

        LoadStage(nextStage);
    }

    /// <summary>
    /// Coroutine handling stage failure and reset to build phase.
    /// </summary>
    private IEnumerator HandleStageFail()
    {
        phase = StagePhase.Failure;

        DisableSources();
        ClearAllBlocks();
        sinkBlock.ResetCounts();

        yield return FlashMessageCoroutine("STAGE FAILED");

        phase = StagePhase.Build;

        timer = currentStage.timeLimit;
        playerInventory.UpdateUnlockedSlots(currentStageIndex);
        hotbarUI.RefreshHotbar();
        objectiveDisplay.ShowStage(currentStage);

        UpdateGoals();
    }

    /// <summary>
    /// The current stage phase.
    /// </summary>
    /// <returns>The current stage phase.</returns>
    public StagePhase CurrentPhase => phase;
}