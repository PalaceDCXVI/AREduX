using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;

    public enum HighlightType
    {
        ObjectHighlight,
        HandHighlight,
        SphericalCursor,
        DotCursor,
        None
    } 

    public HighlightType highlightType = HighlightType.ObjectHighlight;

    //Color selectionAlterationColor = new Color(0.3f, 0.3f, 0.0f, 0.0f);
    Color selectionAlterationColor = new Color(1.0f, 1.0f, 0.0f, 0.0f);

    Color originalHandFillColor;
    Color standardColour;
    Color hoverColour;
    Color contactColour;
    
    string colourPropertyName = "_Color";
    string GrabbableObjectPosPropertyName = "_GrabbableObjectPos";
    string HighlightHandPropertyName = "_HighlightHand";

    GameObject grabbableObject = null;
    SkinnedMeshRenderer handRenderer = null;

    private SpherePointerVisual pointerVisual;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("MaterialManager already exists!");
        }
        
        ColorUtility.TryParseHtmlString("#515151", out originalHandFillColor);
        ColorUtility.TryParseHtmlString("#696F76", out standardColour);
        ColorUtility.TryParseHtmlString("#95963C", out hoverColour);
        ColorUtility.TryParseHtmlString("#A6544C", out contactColour);
    }

    // Update is called once per frame
    void Update()
    {
        if (highlightType == HighlightType.HandHighlight && grabbableObject && handRenderer)
        {
            Vector4 grabbableObjectPos =  grabbableObject.transform.position;
            handRenderer.material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
            Debug.Log(grabbableObjectPos);
            handRenderer.material.SetFloat(HighlightHandPropertyName, 1.0f);
        }
        else
        {
            handRenderer?.material.SetFloat(HighlightHandPropertyName, 0.0f);
        }
    }

    
    public void HandIsFloating(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        switch (highlightType)
        {
        case HighlightType.ObjectHighlight:
            foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
            {
                objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
            }
        break;

        case HighlightType.HandHighlight:
            {
                if (manipulationEventData.Pointer != null)
                {
                    if (manipulationEventData.Pointer.Controller != null)
                    {
                        // If controller is of kind IMixedRealityHand, return average of index and thumb
                        if (manipulationEventData.Pointer.Controller is IMixedRealityHand hand)
                        {
                            if (hand.Visualizer != null)
                            {
                                grabbableObject = null;
                                handRenderer = null;
                                
                                Vector4 grabbableObjectPos = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().worldToLocalMatrix * manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject.transform.position;
                                hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                Debug.Log(grabbableObjectPos);
                                hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat(HighlightHandPropertyName, 0.0f);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetVector(colourPropertyName, originalHandFillColor);
                            }
                        }
                    }
                }
            }
        break;
            
        default:
        break;
        }
    }

    public void HandIsOverObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        switch (highlightType)
        {
        case HighlightType.ObjectHighlight:
            foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
            {
                objectRenderer.material.color = objectRenderer.GetComponent<ObjectReset>().OriginalColor + selectionAlterationColor;
            }
        break;

        case HighlightType.HandHighlight:
            {
                if (manipulationEventData.Pointer != null)
                {
                    if (manipulationEventData.Pointer.Controller != null)
                    {
                        // If controller is of kind IMixedRealityHand, return average of index and thumb
                        if (manipulationEventData.Pointer.Controller is IMixedRealityHand hand)
                        {
                            if (hand.Visualizer != null)
                            {
                                grabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
                                handRenderer = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>();

                                Vector4 grabbableObjectPos = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().worldToLocalMatrix * manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject.transform.position;
                                hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                Debug.Log(grabbableObjectPos);
                                hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat(HighlightHandPropertyName, 1.0f);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor(colourPropertyName, selectionAlterationColor);
                            }
                        }
                    }
                }
            }
        break;
        
        default:
        break;
        }
    }

    public void HandIsTouchingObject(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        switch (highlightType)
        {
        case HighlightType.ObjectHighlight:
            foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
            {
                objectRenderer.material.color = objectRenderer.GetComponent<ObjectReset>().OriginalColor + contactColour;
            }
        break;

        case HighlightType.HandHighlight:
            {
                if (manipulationEventData.Pointer != null)
                {
                    if (manipulationEventData.Pointer.Controller != null)
                    {
                        // If controller is of kind IMixedRealityHand, return average of index and thumb
                        if (manipulationEventData.Pointer.Controller is IMixedRealityHand hand)
                        {
                            if (hand.Visualizer != null)
                            {
                                grabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
                                handRenderer = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>();

                                Vector4 grabbableObjectPos = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().worldToLocalMatrix * manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject.transform.position;
                                hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                Debug.Log(grabbableObjectPos);
                                hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat(HighlightHandPropertyName, 1.0f);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor(colourPropertyName, contactColour);
                            }
                        }
                    }
                }
            }
        break;
        
        default:
        break;
        }
    }
}
