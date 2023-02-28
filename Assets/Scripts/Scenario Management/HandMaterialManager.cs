using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMaterialManager : MonoBehaviour
{
    public bool PseudoHapticsActive = false;

    public Material properMaterial;

    Color selectionAlterationColor = new Color(0.0f, 0.3f, 0.2f, 0.0f);

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
        if (PseudoHapticsActive && manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>())
        {
            manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().material.color = manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>().OriginalColor;
        }

        //SkinnedMeshRenderer handRenderer = manipulationEventData.Pointer.Controller.Visualizer.GameObjectProxy.GetComponentInChildren<RiggedHandVisualizer>().HandRenderer;

        //Debug.Log("Hand is over object: " + manipulationEventData.Pointer.PointerName);
        //Debug.Log("Hand is over object: " + manipulationEventData.Pointer.InputSourceParent.SourceName);
        //Debug.Log("Hand sharedMaterial name: " + handRenderer.sharedMaterial.name);
        //Debug.Log("Shader name: " + handRenderer.sharedMaterial.shader.name);
        //Debug.Log("Property name: " + handRenderer.sharedMaterial.shader.GetPropertyName(1));

        //if (handRenderer.sharedMaterial.HasProperty(colourPropertyName))
        {
            //handRenderer.sharedMaterial.SetColor(colourPropertyName, standardColour);

            //Debug.Log("Colour Value: " + handRenderer.sharedMaterial.GetColor(colourPropertyName));
        }
    }

    public void HandIsOverObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        if (PseudoHapticsActive && manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>())
        {
            manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().material.color = manipulationEventData.ManipulationSource.GetComponent<ObjectReset>().OriginalColor + selectionAlterationColor;
        }


        //SkinnedMeshRenderer handRenderer = manipulationEventData.Pointer.Controller.Visualizer.GameObjectProxy.GetComponentInChildren<RiggedHandVisualizer>().HandRenderer;

        //Debug.Log("Hand is over object: " + manipulationEventData.Pointer.PointerName);
        //Debug.Log("Hand is over object: " + manipulationEventData.Pointer.InputSourceParent.SourceName);
        //Debug.Log("Hand sharedMaterial name: " + handRenderer.sharedMaterial.name);
        //Debug.Log("Shader name: " + handRenderer.sharedMaterial.shader.name);
        //Debug.Log("Property name: " + handRenderer.sharedMaterial.shader.GetPropertyName(1));

        //if (handRenderer.sharedMaterial.HasProperty(colourPropertyName))
        {
            //handRenderer.sharedMaterial.SetColor(colourPropertyName, PseudoHapticsActive ? hoverColour : standardColour);

            //Debug.Log("Colour Value: " + handRenderer.sharedMaterial.GetColor(colourPropertyName));
        }
    }

    public void HandIsTouchingObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        if (PseudoHapticsActive && manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>())
        {
            manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().material.color = manipulationEventData.ManipulationSource.GetComponentInChildren<ObjectReset>().OriginalColor;
        }

        //SkinnedMeshRenderer handRenderer = manipulationEventData.Pointer.Controller.Visualizer.GameObjectProxy.GetComponentInChildren<RiggedHandVisualizer>().HandRenderer;

        //Debug.Log("Hand is over object: " + manipulationEventData.Pointer.PointerName);
        //Debug.Log("Hand is over object: " + manipulationEventData.Pointer.InputSourceParent.SourceName);
        //Debug.Log("Hand sharedMaterial name: " + handRenderer.sharedMaterial.name);
        //Debug.Log("Shader name: " + handRenderer.sharedMaterial.shader.name);
        //Debug.Log("Property name: " + handRenderer.sharedMaterial.shader.GetPropertyName(1));

        //if (handRenderer.sharedMaterial.HasProperty(colourPropertyName))
        {
            //handRenderer.sharedMaterial.SetColor(colourPropertyName, PseudoHapticsActive ? contactColour : standardColour);

            //Debug.Log("Colour Value: " + handRenderer.sharedMaterial.GetColor(colourPropertyName));
        }
    }
}
