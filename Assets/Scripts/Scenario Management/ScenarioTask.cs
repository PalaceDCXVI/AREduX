using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioTask : MonoBehaviour
{
    public string TaskName = "Unnamed Task";

    public UnityEvent OnTaskBegin;

    public UnityEvent OnTaskComplete;

    public int orderIndex;

    public void StartTask()
    {
        OnTaskBegin.Invoke();

        Debug.Log("Task " + TaskName + " Started");
    }

    public void CompleteTask()
    {
        OnTaskComplete.Invoke();

        Debug.Log("Task " + TaskName + " Completed");

        FindObjectOfType<ScenarioManager>().AdvanceCurrentTask();
    }
}
