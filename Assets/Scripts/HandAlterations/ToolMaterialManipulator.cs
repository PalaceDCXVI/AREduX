using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Microsoft.MixedReality.Toolkit.UI.ObjectManipulator))]
public class ToolMaterialManipulator : MonoBehaviour
{
    public ScenarioTask OwningTask;

    public GameObject DotCursorObject;
    public GameObject SphereCursorObject;

    // Start is called before the first frame update
    void Start()
    {
        if (OwningTask)
        {
            OwningTask.OnTaskComplete.AddListener(DisableCursors);
        }

        GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(OnManipulationStarted);
        GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(OnManipulationEnded);

        if (DotCursorObject == null)
        {
            Debug.LogError("Dot Cursor Object not assigned.");
        }

        if (SphereCursorObject == null)
        {
            Debug.LogError("Sphere Cursor Object not assigned.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnManipulationStarted(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        if (OwningTask != null)
        {
            switch (MaterialManager.Instance.highlightType)
            {
                case MaterialManager.HighlightType.ObjectHighlight:
                    break;
                case MaterialManager.HighlightType.HandHighlight:
                    break;
                case MaterialManager.HighlightType.SphericalCursor:
                    if (OwningTask.hasStarted && !OwningTask.hasEnded)
                    {
                        SphereCursorObject.SetActive(true);
                    }
                    break;
                case MaterialManager.HighlightType.DotCursor:
                    if (OwningTask.hasStarted && !OwningTask.hasEnded)
                    {
                        DotCursorObject.SetActive(true);
                    }
                    break;
                case MaterialManager.HighlightType.None:
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (MaterialManager.Instance.highlightType)
            {
                case MaterialManager.HighlightType.ObjectHighlight:
                    break;
                case MaterialManager.HighlightType.HandHighlight:
                    break;
                case MaterialManager.HighlightType.SphericalCursor:
                    SphereCursorObject.SetActive(false);
                    break;
                case MaterialManager.HighlightType.DotCursor:
                    DotCursorObject.SetActive(false);
                    break;
                case MaterialManager.HighlightType.None:
                    break;
                default:
                    break;
            }
        }
    }

    public void OnManipulationEnded(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        if (OwningTask != null)
        {
            switch (MaterialManager.Instance.highlightType)
            {
                case MaterialManager.HighlightType.ObjectHighlight:
                    break;
                case MaterialManager.HighlightType.HandHighlight:
                    break;
                case MaterialManager.HighlightType.SphericalCursor:
                    if (OwningTask.hasStarted && !OwningTask.hasEnded)
                    {
                        SphereCursorObject.SetActive(false);
                    }
                    break;
                case MaterialManager.HighlightType.DotCursor:
                    if (OwningTask.hasStarted && !OwningTask.hasEnded)
                    {
                        DotCursorObject.SetActive(false);
                    }
                    break;
                case MaterialManager.HighlightType.None:
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (MaterialManager.Instance.highlightType)
            {
                case MaterialManager.HighlightType.ObjectHighlight:
                    break;
                case MaterialManager.HighlightType.HandHighlight:
                    break;
                case MaterialManager.HighlightType.SphericalCursor:
                    SphereCursorObject.SetActive(false);
                    break;
                case MaterialManager.HighlightType.DotCursor:
                    DotCursorObject.SetActive(false);
                    break;
                case MaterialManager.HighlightType.None:
                    break;
                default:
                    break;
            }
        }
    }

    public void DisableCursors()
    {
        SphereCursorObject.SetActive(false);
        DotCursorObject.SetActive(false);
    }
}
