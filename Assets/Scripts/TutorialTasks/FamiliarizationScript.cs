using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarizationScript : ScenarioTask
{
    public int TechniquesUsed = 0;

    public List<GameObject> Slots;
    private int MaxFilledSlots = 0;
    public int FilledSlots = 0;

    public AudioClip ObjectHighlightClip;    public string ObjectHighlightClipSubtitle;
    public AudioClip HandHighlightClip;      public string HandHighlightClipSubtitle;
    public AudioClip SphericalCursorClip;    public string SphericalCursorClipSubtitle;
    public AudioClip DotCursorClip;          public string DotCursorClipSubtitle;
    public AudioClip SeethroughClip;         public string SeethroughClipSubtitle;
    public AudioClip NoneClip;               public string NoneClipSubtitle;

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
                AudioPromptManager.Instance.PlayAudioClip(ObjectHighlightClip, ObjectHighlightClipSubtitle);
                break;
            case MaterialManager.HighlightType.HandHighlight:
                AudioPromptManager.Instance.PlayAudioClip(HandHighlightClip, HandHighlightClipSubtitle);
                break;
            case MaterialManager.HighlightType.SphericalCursor:
                AudioPromptManager.Instance.PlayAudioClip(SphericalCursorClip, SphericalCursorClipSubtitle);
                break;
            case MaterialManager.HighlightType.DotCursor:
                AudioPromptManager.Instance.PlayAudioClip(DotCursorClip, DotCursorClipSubtitle);
                break;
            //case MaterialManager.HighlightType.Seethrough:
            //    AudioPromptManager.Instance.PlayAudioClip(SeethroughClip, SeethroughClipSubtitle);
            //    break;
            case MaterialManager.HighlightType.None:
                AudioPromptManager.Instance.PlayAudioClip(NoneClip, NoneClipSubtitle);
                break;
            default:
                break;
        }
    }
}
