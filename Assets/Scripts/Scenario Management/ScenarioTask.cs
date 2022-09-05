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

    public bool hasStarted {get; protected set;}
    public bool hasEnded {get; protected set;}

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        hasEnded = false;
    }

    public virtual void StartTask()
    {
        Debug.Log("Task " + TaskName + " Started");

        OnTaskBegin.Invoke();

        hasStarted = true;
    }

    public virtual void CompleteTask()
    {
        Debug.Log("Task " + TaskName + " Completed");

        OnTaskComplete.Invoke();

        FindObjectOfType<ScenarioManager>().AdvanceCurrentTask();

        hasEnded = true;
    }
}
