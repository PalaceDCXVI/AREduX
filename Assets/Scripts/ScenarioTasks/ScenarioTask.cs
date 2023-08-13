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
    public float audioClipTime = 0.05f;
    public bool audioHasCompleted = false;

    public UnityEvent OnTaskBegin;

    public UnityEvent OnTaskComplete;

    public int orderIndex;

    public bool hasStarted {get; protected set;} = false;
    public bool hasEnded {get; protected set;} = false;


    // Start is called before the first frame update
    virtual protected void Start()
    {
        if (!ScenarioManager.AphasiaAudio && startingAudioClip)
        {
            audioClipTime += startingAudioClip.length;
        }
        else if (ScenarioManager.AphasiaAudio && startingAudioClipWithAphasia)
        {
            audioClipTime += startingAudioClipWithAphasia.length;
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
        OnTaskComplete.Invoke();

        FindObjectOfType<ScenarioManager>().AdvanceCurrentTask();
    
        hasEnded = true;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (hasStarted)
        {
            audioClipTime -= Time.deltaTime;
        }
        if (hasEnded && WaitForAudioClipCompletion && audioClipTime < 0.0f && !audioHasCompleted)
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
