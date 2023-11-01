using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Audio;
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
        Seethrough,
        None
    } 

    public HighlightType highlightType = HighlightType.Seethrough;

    //Color selectionAlterationColor = new Color(0.3f, 0.3f, 0.0f, 0.0f);
    Color selectionAlterationColor = new Color(1.0f, 1.0f, 0.0f, 0.0f);

    Color originalHandFillColor;
    Color standardColour;
    Color hoverColour;
    Color contactColour;
    public float SeethroughAlpha = 0.7f;
    
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

    public void SetHighlightTypeToObject()
    {
        highlightType = HighlightType.ObjectHighlight;
    }

    public void SetHighlightTypeToHand()
    {
        highlightType = HighlightType.HandHighlight;
    }

    public void SetHighlightTypeToDot()
    {
        highlightType = HighlightType.DotCursor;
    }

    public void SetHighlightTypeToSpherical()
    {
        highlightType = HighlightType.SphericalCursor;
    }

    public void SetHighlightTypeToNone()
    {
        highlightType = HighlightType.None;
    }

    public static void ToOpaqueMode(Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
 
    public static void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
    }

    public static void ToTransparentMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0.0f);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
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

        case HighlightType.Seethrough:
            foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
            {
                foreach (var mat in objectRenderer.materials)
                {
                    //if material rendering mode is not set to transparent already
                    if (!(mat.GetInt("_SrcBlend") == (int) UnityEngine.Rendering.BlendMode.One && mat.GetFloat("_ZWrite") == 0.0f))
                    {
                        ToOpaqueMode(mat);
                    }
                }
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

        case HighlightType.Seethrough:
            foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
            {
                foreach (var mat in objectRenderer.materials)
                {
                    //if material rendering mode is not set to transparent already
                    if (!(mat.GetInt("_SrcBlend") == (int) UnityEngine.Rendering.BlendMode.One && mat.GetFloat("_ZWrite") == 0.0f))
                    {
                        ToFadeMode(mat);
                        mat.color = new Color(objectRenderer.GetComponent<ObjectReset>().OriginalColor.r, objectRenderer.GetComponent<ObjectReset>().OriginalColor.g, objectRenderer.GetComponent<ObjectReset>().OriginalColor.b, SeethroughAlpha);
                    }
                }
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
        
        case HighlightType.Seethrough:
            foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
            {
                foreach (var mat in objectRenderer.materials)
                {
                    //if material rendering mode is not set to transparent already
                    if (!(mat.GetInt("_SrcBlend") == (int) UnityEngine.Rendering.BlendMode.One && mat.GetFloat("_ZWrite") == 0.0f))
                    {
                        ToFadeMode(mat);
                        mat.color = new Color(objectRenderer.GetComponent<ObjectReset>().OriginalColor.r, objectRenderer.GetComponent<ObjectReset>().OriginalColor.g, objectRenderer.GetComponent<ObjectReset>().OriginalColor.b, SeethroughAlpha);

                    }
                }
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
