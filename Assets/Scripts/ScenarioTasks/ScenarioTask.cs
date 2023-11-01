using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioTask : MonoBehaviour
{
    public string TaskName = "Unnamed Task";

    public AudioClip startingAudioClip;
    public AudioClip startingAudioClipWithAphasia;
    
    public bool WaitForAudioClipCompletion = true;
    public float audioClipTime = 0.00f;
    private float internalAudioClipTime = 0.00f;
    public bool audioHasCompleted = false;

    public UnityEvent OnTaskBegin;

    public UnityEvent OnTaskComplete;

    public UnityEvent OnTaskReset;

    public int orderIndex;

    public bool hasStarted {get; protected set;} = false;
    public bool hasEnded {get; protected set;} = false;
    public bool IsTutorialTask = false;


    // Start is called before the first frame update
    virtual protected void Start()
    {
        internalAudioClipTime = audioClipTime;

        if (!ScenarioManager.AphasiaAudio && startingAudioClip)
        {
            internalAudioClipTime += startingAudioClip.length;
        }
        else if (ScenarioManager.AphasiaAudio && startingAudioClipWithAphasia)
        {
            internalAudioClipTime += startingAudioClipWithAphasia.length;
        }
    }

    public virtual void StartTask()
    {
        Debug.Log("Task " + TaskName + " Started");

        if (!ScenarioManager.AphasiaAudio)
        {
            if (startingAudioClip != null)
            {
                AudioPromptManager.Instance.PlayAudioClip(startingAudioClip);
            }
        }
        else
        {
            if (startingAudioClipWithAphasia != null)
            {
                AudioPromptManager.Instance.PlayAudioClip(startingAudioClipWithAphasia);
            }
        }

        OnTaskBegin.Invoke();

        hasStarted = true;
    }

    private void Internal_CompleteTask()
    {
        hasEnded = true;
        Debug.Log("Task: " + TaskName + " has completed");
        OnTaskComplete.Invoke();

        FindObjectOfType<ScenarioManager>().AdvanceCurrentTask();
    
    }

    public virtual void ResetScenario()
    {  
        hasStarted = false;
        hasEnded = false;
        internalAudioClipTime = audioClipTime;
        audioHasCompleted = false;

        OnTaskReset.Invoke();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (hasStarted)
        {
            internalAudioClipTime -= Time.deltaTime;
        }
        if (hasEnded && WaitForAudioClipCompletion && internalAudioClipTime <= 0.0f && !audioHasCompleted)
        {
            audioHasCompleted = true;
            Internal_CompleteTask();
        }
    }

    public virtual void CompleteTask()
    {
        if (!hasEnded)
        {
            if (WaitForAudioClipCompletion)
            {
                hasEnded = true;
            }
            else
            {
                Internal_CompleteTask();
            }
        }
    }
}
