using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioDebugText : MonoBehaviour
{

    private ScenarioManager manager;

    private TMPro.TextMeshProUGUI text;

    private string originalText;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();

        manager = FindObjectOfType<ScenarioManager>();

        if (manager.ScenarioObjectives.Count > 0)
        {
            text.text = "";
        }

        foreach(ScenarioTask task in manager.ScenarioObjectives)
        {
            text.text += task.TaskName + "\n";
        }

        originalText = text.text;
    }

    // Update is called once per frame
    public void UpdateText()
    {
        Debug.Log("Updating log");

        text.text = originalText;
        
        int currentIndex = -1;
        if (manager.currentTask != null)
        {
            currentIndex = text.text.IndexOf(manager.currentTask.TaskName);
        }
        
        //if we're finished alltasks, strike through all elements.
        if (currentIndex == -1)
        {
            if (manager.finishedTasks)
            {
                text.text = text.text.Insert(0, "<s>");
                text.text = text.text.Insert(text.text.Length-1, "</s>");
            }
        }
        else
        {
            text.text = text.text.Insert(0, "<s>");
            text.text = text.text.Insert(text.text.IndexOf(manager.currentTask.TaskName), "</s>");
        }
    }
}
