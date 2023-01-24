using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMaterialManager : MonoBehaviour
{
    public bool PseudoHapticsActive = false;

    Color standardColour;
    Color hoverColour;
    Color contactColour;
    // Start is called before the first frame update
    void Start()
    {
         ColorUtility.TryParseHtmlString("#696F76", out standardColour);
         ColorUtility.TryParseHtmlString("#95963C", out hoverColour);
         ColorUtility.TryParseHtmlString("#A6544C", out contactColour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePseudoHaptics(TextMesh displayText)
    {
        PseudoHapticsActive = !PseudoHapticsActive;

        if (displayText)
        {
            displayText.text = "Pseudo Haptics: " + (PseudoHapticsActive ? "On" : "Off");
        }
    }

    public void HandIsFloating(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        if (PseudoHapticsActive)
        {
            Debug.Log("Hand has left object " + manipulationEventData.Pointer.PointerName);
            Debug.Log("Hand is over object " + manipulationEventData.Pointer.InputSourceParent.SourceName);
            MonoBehaviour inputBehaviour;
            if (manipulationEventData.Pointer.Controller.Visualizer.TryGetMonoBehaviour(out inputBehaviour))
            {
                inputBehaviour.GetComponentInChildren<Renderer>().material.color = standardColour;
            }
        }
        else
        {
            MonoBehaviour inputBehaviour;
            if (manipulationEventData.Pointer.Controller.Visualizer.TryGetMonoBehaviour(out inputBehaviour))
            {
                inputBehaviour.GetComponentInChildren<Renderer>().material.color = standardColour;
            }
        }
    }

    public void HandIsOverObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        if (PseudoHapticsActive)
        {
            Debug.Log("Hand is over object " + manipulationEventData.Pointer.PointerName);
            Debug.Log("Hand is over object " + manipulationEventData.Pointer.InputSourceParent.SourceName);
            MonoBehaviour inputBehaviour;
            if (manipulationEventData.Pointer.Controller.Visualizer.TryGetMonoBehaviour(out inputBehaviour))
            {
                inputBehaviour.GetComponentInChildren<Renderer>().material.color = hoverColour;
            }
        }
        else
        {
            MonoBehaviour inputBehaviour;
            if (manipulationEventData.Pointer.Controller.Visualizer.TryGetMonoBehaviour(out inputBehaviour))
            {
                inputBehaviour.GetComponentInChildren<Renderer>().material.color = standardColour;
            }
        }
    }

    public void HandIsTouchingObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        if (PseudoHapticsActive)
        {
            Debug.Log("Hand is touching object " + manipulationEventData.Pointer.PointerName);
            Debug.Log("Hand is over object " + manipulationEventData.Pointer.InputSourceParent.SourceName);
            MonoBehaviour inputBehaviour;
            if (manipulationEventData.Pointer.Controller.Visualizer.TryGetMonoBehaviour(out inputBehaviour))
            {
                inputBehaviour.GetComponentInChildren<Renderer>().material.color = contactColour;
            }
        }
        else
        {
            MonoBehaviour inputBehaviour;
            if (manipulationEventData.Pointer.Controller.Visualizer.TryGetMonoBehaviour(out inputBehaviour))
            {
                inputBehaviour.GetComponentInChildren<Renderer>().material.color = standardColour;
            }
        }
    }
}
