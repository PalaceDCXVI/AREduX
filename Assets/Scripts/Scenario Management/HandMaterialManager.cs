using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMaterialManager : MonoBehaviour
{
    public static bool PseudoHapticsActive = false;

    public Material properMaterial;

    Color selectionAlterationColor = new Color(0.3f, 0.3f, 0.0f, 0.0f);

    Color standardColour;
    Color hoverColour;
    Color contactColour;
    
    string colourPropertyName = "_Fill_Color_";

    // Start is called before the first frame update
    void Start()
    {
         ColorUtility.TryParseHtmlString("#696F76", out standardColour);
         ColorUtility.TryParseHtmlString("#95963C", out hoverColour);
         ColorUtility.TryParseHtmlString("#A6544C", out contactColour);


        if (PlayerPrefs.HasKey(PseudoHapticsSwitch.PseudoHapticsEnabledKey))
        {
            PseudoHapticsActive = PlayerPrefs.GetInt(PseudoHapticsSwitch.PseudoHapticsEnabledKey) > 0 ? true : false;
        }
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
        foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
        {
            if (PseudoHapticsActive)
            {
                objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
            }
        }

        //if (PseudoHapticsActive && manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>())
        //{
        //    manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().material.color = manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>().OriginalColor;
        //}
    }

    public void HandIsOverObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
        {
            if (PseudoHapticsActive && objectRenderer.GetComponentInChildren<ObjectReset>())
            {
                objectRenderer.material.color = objectRenderer.GetComponent<ObjectReset>().OriginalColor + selectionAlterationColor;
            }
        }

        //if (PseudoHapticsActive && manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>())
        //{
        //    manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().material.color = manipulationEventData.ManipulationSource.GetComponent<ObjectReset>().OriginalColor + selectionAlterationColor;
        //}
    }

    public void HandIsTouchingObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
        {
            if (PseudoHapticsActive)
            {
                objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
            }
        }

        //if (PseudoHapticsActive && manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>())
        //{
        //    manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().material.color = manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>().OriginalColor;
        //}
    }
}
