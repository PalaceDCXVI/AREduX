using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FamiliarizationScript : ScenarioTask
{
    public int TechniquesUsed = 0;

    public List<GameObject> Slots;
    private int MaxFilledSlots = 0;
    public int FilledSlots = 0;
    public List<GameObject> Spheres;

    public bool ForkIsInPlace = false;
    public List<GameObject> CutSlots;
    private int MaxFilledCutSlots = 0;
    public int SlotsThatHaveBeenCut = 0;
    public GameObject Part2ObjectsParent = null;

    public AudioClip Part2IntroClip;        public string Part2IntroClipSubtitle;

    public AudioClip ObjectHighlightClip;   public string ObjectHighlightClipSubtitle;
    public AudioClip ObjectHighlight2Clip;  public string ObjectHighlightClip2Subtitle;

    public AudioClip HandHighlightClip;     public string HandHighlightClipSubtitle;
    public AudioClip HandHighlight2Clip;    public string HandHighlightClip2Subtitle;

    public AudioClip SphericalCursorClip;   public string SphericalCursorClipSubtitle;
    public AudioClip SphericalCursor2Clip;  public string SphericalCursorClip2Subtitle;

    public AudioClip DotCursorClip;         public string DotCursorClipSubtitle;
    public AudioClip DotCursor2Clip;        public string DotCursorClip2Subtitle;

    public AudioClip SeethroughClip;        public string SeethroughClipSubtitle;
    public AudioClip Seethrough2Clip;       public string SeethroughClip2Subtitle;

    public AudioClip NoneClip;              public string NoneClipSubtitle;
    public AudioClip NoneClip2;             public string NoneClip2Subtitle;

    override protected void Start()
    {
        base.Start();
        IsTutorialTask = true;
        ScenarioManager.Instance.InTutorial = true;

        MaxFilledSlots = Slots.Count;

        MaxFilledCutSlots = CutSlots.Count;
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
            foreach (GameObject slot in Slots)
            {
                slot.SetActive(false);
            }
            foreach (GameObject sphere in Spheres)
            {
                sphere.SetActive(false);
            }

            if (TechniquesUsed == 0)
            {
                AudioPromptManager.Instance.PlayAudioClip(Part2IntroClip, Part2IntroClipSubtitle);
            }

            Part2ObjectsParent.SetActive(true);
            StartCoroutine(PlayCurrentPart2Prompt(TechniquesUsed == 0));
        }
    }

    public void IncrementCutSlots()
    {
        SlotsThatHaveBeenCut++;
        if (MaxFilledCutSlots == SlotsThatHaveBeenCut)
        {
            TechniquesUsed++;

            if (TechniquesUsed > (int)MaterialManager.HighlightType.None)
            {
                CompleteTask();
            }
            else
            {
                foreach (GameObject slot in Slots)
                {
                    slot.SetActive(true);
                }
                foreach (GameObject sphere in Spheres)
                {
                    sphere.SetActive(true);
                }
                foreach (GameObject cube in CutSlots)
                {
                    cube.SetActive(true);
                }
                foreach (ObjectReset item in GetComponentsInChildren<ObjectReset>())
                {
                    item.ResetObject();
                }

                Part2ObjectsParent.SetActive(false);
                FilledSlots = 0;
                SlotsThatHaveBeenCut = 0;
                ForkIsInPlace = false;
                MaterialManager.Instance.IncrementHighlightType(true);
                MaterialManager.Instance.SetHighlightTypeToCurrent();
                PlayCurrentTechniquePrompt();
            }
        }
    }

    public void PlayCurrentTechniquePrompt()
    {
        StopCoroutine(PlayCurrentPart2Prompt());

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

    public IEnumerator PlayCurrentPart2Prompt(bool waitForSilence = false)
    {
        if (waitForSilence)
        {
            yield return new WaitUntil(() => AudioPromptManager.Instance.audioSource.isPlaying == false);
        }

        switch (MaterialManager.Instance.highlightType)
        {
            case MaterialManager.HighlightType.ObjectHighlight:
                AudioPromptManager.Instance.PlayAudioClip(ObjectHighlight2Clip, ObjectHighlightClip2Subtitle);
                break;
            case MaterialManager.HighlightType.HandHighlight:
                AudioPromptManager.Instance.PlayAudioClip(HandHighlight2Clip, HandHighlightClip2Subtitle);
                break;
            case MaterialManager.HighlightType.SphericalCursor:
                AudioPromptManager.Instance.PlayAudioClip(SphericalCursor2Clip, SphericalCursorClip2Subtitle);
                break;
            case MaterialManager.HighlightType.DotCursor:
                AudioPromptManager.Instance.PlayAudioClip(DotCursor2Clip, DotCursorClip2Subtitle);
                break;
            //case MaterialManager.HighlightType.Seethrough:
            //    AudioPromptManager.Instance.PlayAudioClip(SeethroughClip, SeethroughClipSubtitle);
            //    break;
            case MaterialManager.HighlightType.None:
                AudioPromptManager.Instance.PlayAudioClip(NoneClip2, NoneClip2Subtitle);
                break;
            default:
                break;
        }

        yield return false;
    }
}
