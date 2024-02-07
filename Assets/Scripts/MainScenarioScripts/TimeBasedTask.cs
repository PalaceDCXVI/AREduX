using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBasedTask : ScenarioTask
{
    public float TaskTime = 1.0f;
    public bool UseAudioClipTime;
    // Start is called before the first frame update
    void Start()
    {
        WaitForAudioClipCompletion = false;
        if (UseAudioClipTime)
        {
            if (!ScenarioManager.AphasiaAudio)
            {
                if (TutorialAudio != null && ScenarioManager.Instance.InTutorial)
                {
                    TaskTime = TutorialAudio.length;
                }
                else if (StartingAudioClip != null)
                {
                    TaskTime = StartingAudioClip.length;
                }
            }
            else
            {
                if (StartingAudioClipWithAphasia)
                {
                    TaskTime = StartingAudioClipWithAphasia.length;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !hasEnded)
        {
            TaskTime -= Time.deltaTime;
            if (TaskTime <= 0.0f)
            {
                CompleteTask();
            }
        }
    }
}
