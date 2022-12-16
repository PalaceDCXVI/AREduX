using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//To use this class, add the scenario manager prefab to the scene and add the debug canvas as a child of the main camera if you wish to see debug info.

//For the scenario system itself, add a ScenarioTask to any object, provide it a task name and index in the scenario. No tasks may have the same index.
//The first task will immediately have its start task functions called.
//To complete a task, something, like an object manipulate end event, will have to call the task's CompleteTask function.

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance;

    public static bool AphasiaAudio = false;

    public List<ScenarioTask> ScenarioObjectives;

    public ScenarioTask currentTask;
    public bool ScenarioHasStarted { get; private set; }
    public bool finishedTasks = false;

    public UnityEvent OnScenarioEnd;

    // Gather all existing scenario tasks in the scene.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("ScenarioManager already exists.");
            return;
        }

        ScenarioHasStarted = false;

        var tasks = FindObjectsOfType<ScenarioTask>(true);

        ScenarioObjectives.Clear();
        foreach(var task in tasks)
        {
            ScenarioObjectives.Add(task.GetComponent<ScenarioTask>());
        }

        ScenarioObjectives.Sort((ScenarioTask x, ScenarioTask y) =>
        {
            if (x.orderIndex > y.orderIndex) 
            {
                return 1;
            }
            else if (x.orderIndex < y.orderIndex) 
            {
                return -1;
            }
            else 
            {
                Debug.LogError("Scenario Tasks " + x.TaskName + " and " + y.TaskName + " have the same index!");
                return 0;
            }
        });

        if (ScenarioObjectives.Count > 0)
        {
            currentTask = ScenarioObjectives[0];
            //currentTask.StartTask();
        }
        else
        {
            currentTask = null;
        }
    }

    public void AdvanceCurrentTask()
    {
        if (finishedTasks)
        {
            return;
        }
        if (currentTask == null)
        {
            Debug.LogError("Trying to advance task when current task is null!");
            return;
        }
        
        int newIndex = ScenarioObjectives.IndexOf(currentTask) + 1;
        currentTask = (newIndex) < ScenarioObjectives.Count ? ScenarioObjectives[newIndex] : null;

        if (currentTask == null)
        {
            finishedTasks = true;
            OnScenarioEnd.Invoke();
        }

        currentTask?.StartTask();

        //if debug text exists, update it.
        FindObjectOfType<ScenarioDebugText>()?.UpdateText();
    }
    public void StartScenario()
    {
        ScenarioHasStarted = true;
    }

    public void ResetScenario()
    {
        finishedTasks = false; 
        var tasks = FindObjectsOfType<ScenarioTask>();

        ScenarioObjectives.Clear();
        foreach (var task in tasks)
        {
            ScenarioObjectives.Add(task.GetComponent<ScenarioTask>());
        }

        ScenarioObjectives.Sort((ScenarioTask x, ScenarioTask y) =>
        {
            if (x.orderIndex > y.orderIndex)
            {
                return 1;
            }
            else if (x.orderIndex < y.orderIndex)
            {
                return -1;
            }
            else
            {
                Debug.LogError("Scenario Tasks " + x.TaskName + " and " + y.TaskName + " have the same index!");
                return 0;
            }
        });

        if (ScenarioObjectives.Count > 0)
        {
            currentTask = ScenarioObjectives[0];
            //currentTask.StartTask();
        }
        else
        {
            currentTask = null;
        }
    }

    public void ToggleAphasiaAudio(TextMesh displayText)
    {
        AphasiaAudio = !AphasiaAudio;

        if (displayText)
        {
            displayText.text = "Dementia Affected Audio: " + (AphasiaAudio ? "On" : "Off");
        }
    }

}
