using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarizationScript : ScenarioTask
{
    public int TechniquesUsed = 0;

    public List<GameObject> Slots;
    private int MaxFilledSlots = 0;
    public int FilledSlots = 0;

    public AudioClip ObjectHighlightClip;
    public AudioClip HandHighlightClip;
    public AudioClip SphericalCursorClip;
    public AudioClip DotCursorClip;
    public AudioClip SeethroughClip;
    public AudioClip NoneClip;

    override protected void Start()
    {
        base.Start();
        IsTutorialTask = true;
        ScenarioManager.Instance.InTutorial = true;

        MaxFilledSlots = Slots.Count;
    }

    override protected void Update()
    {
        base.Update();

    }

    public void StartFamiliarizationSimulation()
    {
        MaterialManager.Instance.SetCurrentHighlightIndexAndApply(0);
        PlayCurrentTechniquePrompt();
    }

    public void IncrementFilledSlots()
    {
        FilledSlots++;
        if (MaxFilledSlots == FilledSlots)
        {
            TechniquesUsed++;

            if (TechniquesUsed > (int)MaterialManager.HighlightType.None)
            {
                CompleteTask();
            }
            else
            {
                foreach (ObjectReset item in GetComponentsInChildren<ObjectReset>())
                {
                    item.ResetObject();
                }
                FilledSlots = 0;
                MaterialManager.Instance.IncrementHighlightType(true);
                MaterialManager.Instance.SetHighlightTypeToCurrent();
                PlayCurrentTechniquePrompt();
            }
        }
    }

    public void PlayCurrentTechniquePrompt()
    {
        switch (MaterialManager.Instance.highlightType)
        {
            case MaterialManager.HighlightType.ObjectHighlight:
                AudioPromptManager.Instance.PlayAudioClip(ObjectHighlightClip);
                break;
            case MaterialManager.HighlightType.HandHighlight:
                AudioPromptManager.Instance.PlayAudioClip(HandHighlightClip);
                break;
            case MaterialManager.HighlightType.SphericalCursor:
                AudioPromptManager.Instance.PlayAudioClip(SphericalCursorClip);
                break;
            case MaterialManager.HighlightType.DotCursor:
                AudioPromptManager.Instance.PlayAudioClip(DotCursorClip);
                break;
            case MaterialManager.HighlightType.Seethrough:
                AudioPromptManager.Instance.PlayAudioClip(SeethroughClip);
                break;
            case MaterialManager.HighlightType.None:
                AudioPromptManager.Instance.PlayAudioClip(NoneClip);
                break;
            default:
                break;
        }
    }
}
