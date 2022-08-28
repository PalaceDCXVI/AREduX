using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To use this class, add the scenario manager prefab to the scene and add the debug canvas as a child of the main camera if you wish to see debug info.

//For the scenario system itself, add a ScenarioTask to any object, provide it a task name and index in the scenario. No tasks may have the same index.
//The first task will immediately have its start task functions called.
//To complete a task, something, like an object manipulate end event, will have to call the task's CompleteTask function.

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance;

    public List<ScenarioTask> ScenarioObjectives;

    public ScenarioTask currentTask;
    public bool finishedTasks = false;

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

        var tasks = FindObjectsOfType<ScenarioTask>();

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
            currentTask.StartTask();
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
        }

        currentTask?.StartTask();

        //if debug text exists, update it.
        FindObjectOfType<ScenarioDebugText>()?.UpdateText();
    }
}
