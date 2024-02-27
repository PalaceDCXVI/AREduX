using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Audio;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;

    [Serializable]
    public enum HighlightType
    {
        ObjectHighlight = 0,
        HandHighlight = 1,
        SphericalCursor = 2,
        DotCursor = 3,
        None = 4,
        //Seethrough = int.MaxValue //Not used anymore.
    } 

    public HighlightType highlightType = HighlightType.None;

    //Color selectionAlterationColor = new Color(0.3f, 0.3f, 0.0f, 0.0f);
    Color selectionAlterationColor = new Color(1.0f, 1.0f, 0.0f, 0.0f);
    public Color ObjectHighlightColor => selectionAlterationColor;

    Color originalHandFillColor;
    Color standardColour;
    Color hoverColour;
    Color contactColour;
    public float SeethroughAlpha = 0.7f;
    
    string colourPropertyName = "_Color";
    public static string GrabbableObjectPosPropertyName = "_GrabbableObjectPos";
    public static string HighlightHandPropertyName = "_HighlightHand";

    GameObject grabbableObject = null;
    GameObject RightGrabbableObject = null;
    GameObject LeftGrabbableObject = null;
    SkinnedMeshRenderer handRenderer = null;
    SkinnedMeshRenderer RightHandRenderer = null;
    SkinnedMeshRenderer LeftHandRenderer = null;

    private SpherePointerVisual pointerVisual;

    public List<HighlightType> OrderOfHighlightTypes;
    public int currentHighlightIndex = 0;

    public GameObject HandRendererDeathPanel = null;

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

        for (int i = 0; i <= (int)HighlightType.None; i++)
        {
            OrderOfHighlightTypes.Add((HighlightType)i);
        }
        //OrderOfHighlightTypes.Remove(HighlightType.Seethrough);

        GenerateRandomLoop(OrderOfHighlightTypes);
    }

    // Update is called once per frame
    void Update()
    {
        if (HandRendererDeathPanel.activeSelf)
        {
            handRenderer = null;
            HandRendererDeathPanel.SetActive(false);
        }

        if (highlightType == HighlightType.HandHighlight && grabbableObject && handRenderer)
        {
            Vector4 grabbableObjectPos =  grabbableObject.transform.position;
            handRenderer.material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
            Debug.Log(grabbableObjectPos);
            handRenderer.material.SetFloat(HighlightHandPropertyName, 1.0f);
        }
        else
        {
            if (!(handRenderer == null || handRenderer.Equals(null)))
            {
                handRenderer.material.SetFloat(HighlightHandPropertyName, 0.0f);
            }
        }

        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, (highlightType == HighlightType.HandHighlight && grabbableObject) ? 1.0f : 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, (highlightType == HighlightType.HandHighlight && grabbableObject) ? 1.0f : 0.0f);

        if (RightHandRenderer != null && RightGrabbableObject != null)
        {
            RightHandRenderer.material.SetVector(GrabbableObjectPosPropertyName, RightGrabbableObject.transform.position);
        }
        if (LeftHandRenderer != null && LeftGrabbableObject != null)
        {
            LeftHandRenderer.material.SetVector(GrabbableObjectPosPropertyName, LeftGrabbableObject.transform.position);
        }
    }

    public List<HighlightType> GenerateRandomLoop(List<HighlightType> listToShuffle)
    {
        for (int i = listToShuffle.Count - 1; i > 0; i--)
        {
            var k = UnityEngine.Random.Range(0, i + 1);
            var value = listToShuffle[k];
            listToShuffle[k] = listToShuffle[i];
            listToShuffle[i] = value;
        }
        return listToShuffle;
    }

    public void SetHighlightType(HighlightType type)
    {
        highlightType = type;
        currentHighlightIndex = OrderOfHighlightTypes.IndexOf(highlightType);

        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, highlightType == HighlightType.HandHighlight ? 1.0f : 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, highlightType == HighlightType.HandHighlight ? 1.0f : 0.0f);
    }

    public void SetHighlightTypeToCurrent()
    {
        if (currentHighlightIndex >= OrderOfHighlightTypes.Count || currentHighlightIndex < 0)
        {
            Debug.Log("index: " + currentHighlightIndex + " is outside of range: " + OrderOfHighlightTypes.Count);
        }
        else
        {
            highlightType = OrderOfHighlightTypes[currentHighlightIndex];
        }

        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, highlightType == HighlightType.HandHighlight ? 1.0f : 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, highlightType == HighlightType.HandHighlight ? 1.0f : 0.0f);
    }

    public void IncrementHighlightType(bool bypassTutorialChange = false)
    {
        if (!ScenarioManager.Instance.InTutorial || bypassTutorialChange)
        {
            currentHighlightIndex++;
        }
    }

    public void SetCurrentHighlightIndex(int currentIndex)
    {
        currentHighlightIndex = currentIndex;
    }

    public void SetCurrentHighlightIndexAndApply(int currentIndex)
    {
        currentHighlightIndex = currentIndex;
        SetHighlightTypeToCurrent();
    }

    public void SetHighlightTypeToObject()
    {
        highlightType = HighlightType.ObjectHighlight;
        currentHighlightIndex = OrderOfHighlightTypes.IndexOf(highlightType);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
    }

    public void SetHighlightTypeToHand()
    {
        highlightType = HighlightType.HandHighlight;
        currentHighlightIndex = OrderOfHighlightTypes.IndexOf(highlightType);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 1.0f);
    }

    public void SetHighlightTypeToDot()
    {
        highlightType = HighlightType.DotCursor;
        currentHighlightIndex = OrderOfHighlightTypes.IndexOf(highlightType);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
    }

    public void SetHighlightTypeToSpherical()
    {
        highlightType = HighlightType.SphericalCursor;
        currentHighlightIndex = OrderOfHighlightTypes.IndexOf(highlightType);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
    }

    public void SetHighlightTypeToSeeThrough()
    {
        //ighlightType = HighlightType.Seethrough;
        currentHighlightIndex = OrderOfHighlightTypes.IndexOf(highlightType);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
    }

    public void SetHighlightTypeToNone()
    {
        highlightType = HighlightType.None;
        currentHighlightIndex = OrderOfHighlightTypes.IndexOf(highlightType);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
        CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
    }

    public static void ToOpaqueMode(Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        //material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        //material.renderQueue = -1;
        material.SetFloat("_Mode", 0.0f);
    }
 
    public static void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        //material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent+1;
        material.SetFloat("_Mode", 2.0f);
    }

    public static void ToTransparentMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0.0f);
        //material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent+1;
        material.SetFloat("_Mode", 2.0f);
    }

    public void HandIsFloating(Microsoft.MixedReality.Toolkit.UI.ManipulationEventData manipulationEventData)
    {
        switch (highlightType)
        {
        case HighlightType.ObjectHighlight:
            foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
            {
                if (objectRenderer.GetComponent<ObjectReset>())
                {
                    objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
                }
            }
        break;

        //case HighlightType.Seethrough:
        //    foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
        //    {
        //        if (objectRenderer.GetComponent<ObjectReset>())
        //        {
        //            foreach (var mat in objectRenderer.materials)
        //            {
        //                //if material rendering mode is not set to transparent already
        //                if (!(mat.GetInt("_SrcBlend") == (int)UnityEngine.Rendering.BlendMode.One && mat.GetFloat("_ZWrite") == 0.0f))
        //                {
        //                    //ToOpaqueMode(mat);
        //                }
        //            }
        //            objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
        //        }
        //    }
        //break;

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
                                GameObject localGrabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
                                SkinnedMeshRenderer localHandRenderer = hand.Visualizer.GameObjectProxy.GetComponentsInChildren<SkinnedMeshRenderer>().Single(hr => hr.material.HasProperty(GrabbableObjectPosPropertyName));

                                if (localHandRenderer == null)
                                {
                                    Debug.LogError("Local Hand Renderer doesn't have the right material???");
                                }

                                if (hand.ControllerHandedness == Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right)
                                {
                                    RightGrabbableObject = null;
                                    RightHandRenderer = null;
                                }
                                else
                                {
                                    LeftGrabbableObject = null;
                                    LeftHandRenderer = null;
                                }

                                grabbableObject = null;
                                handRenderer = null;
                                
                                Vector4 grabbableObjectPos = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().worldToLocalMatrix * manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject.transform.position;
                                ///hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
                                Debug.Log(grabbableObjectPos);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<RiggedHandVisualizer>().HandMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<RiggedHandVisualizer>().HandMaterial.SetFloat(HighlightHandPropertyName, 0.0f);

                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);

                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 0.0f);
                                    
                                localHandRenderer.material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                localHandRenderer.material.SetFloat(HighlightHandPropertyName, 0.0f);

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
                if (objectRenderer.GetComponent<ObjectReset>())
                {
                    objectRenderer.material.color = objectRenderer.GetComponent<ObjectReset>().OriginalColor + selectionAlterationColor;
                }
            }
        break;

        //case HighlightType.Seethrough:
        //    foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
        //    {
        //        foreach (var mat in objectRenderer.materials)
        //        {
        //            if (objectRenderer.GetComponent<ObjectReset>())
        //            {
        //                //if material rendering mode is not set to transparent already
        //                if (!(mat.GetInt("_SrcBlend") == (int)UnityEngine.Rendering.BlendMode.One && mat.GetFloat("_ZWrite") == 0.0f))
        //                {
        //                    ToFadeMode(mat);
        //                    mat.color = new Color(objectRenderer.GetComponent<ObjectReset>().OriginalColor.r, objectRenderer.GetComponent<ObjectReset>().OriginalColor.g, objectRenderer.GetComponent<ObjectReset>().OriginalColor.b, SeethroughAlpha);
        //                }
        //            }
        //        }
        //    }
        //break;

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
                                GameObject localGrabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
                                SkinnedMeshRenderer localHandRenderer = hand.Visualizer.GameObjectProxy.GetComponentsInChildren<SkinnedMeshRenderer>().Single(hr => hr.material.HasProperty(GrabbableObjectPosPropertyName));

                                if(localHandRenderer == null)
                                {
                                    Debug.LogError("Local Hand Renderer doesn't have the right material???");
                                }

                                if (hand.ControllerHandedness == Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right)
                                {
                                    RightGrabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
                                    RightHandRenderer = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>();
                                }
                                else
                                {
                                    LeftGrabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
                                    LeftHandRenderer = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>();
                                }

                                grabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
                                handRenderer = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>();

                                Vector4 grabbableObjectPos = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().worldToLocalMatrix * manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject.transform.position;
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetFloat(HighlightHandPropertyName, 1.0f);
                                Debug.Log(grabbableObjectPos);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<RiggedHandVisualizer>().HandMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                //hand.Visualizer.GameObjectProxy.GetComponentInChildren<RiggedHandVisualizer>().HandMaterial.SetFloat(HighlightHandPropertyName, 1.0f);

                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.RiggedHandMeshMaterial.SetFloat(HighlightHandPropertyName, 1.0f);

                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile.SystemHandMeshMaterial.SetFloat(HighlightHandPropertyName, 1.0f);

                                localHandRenderer.material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
                                localHandRenderer.material.SetFloat(HighlightHandPropertyName, 1.0f);
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
        if (SimulationDataManager.Instance)
        {
            SimulationDataManager.Instance.AddGrasp(Vector3.Distance(manipulationEventData.ManipulationSource.transform.position, manipulationEventData.PointerCentroid), manipulationEventData.Pointer.SphereCastRadius);
        }

        //switch (highlightType)
        //{
        //case HighlightType.ObjectHighlight:
        //    foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
        //    {
        //        objectRenderer.material.color = objectRenderer.GetComponent<ObjectReset>().OriginalColor + contactColour;
        //    }
        //break;
        //
        //case HighlightType.Seethrough:
        //    foreach (var objectRenderer in manipulationEventData.ManipulationSource.GetComponentsInChildren<Renderer>())
        //    {
        //        foreach (var mat in objectRenderer.materials)
        //        {
        //            //if material rendering mode is not set to transparent already
        //            if (!(mat.GetInt("_SrcBlend") == (int) UnityEngine.Rendering.BlendMode.One && mat.GetFloat("_ZWrite") == 0.0f))
        //            {
        //                ToFadeMode(mat);
        //                mat.color = new Color(objectRenderer.GetComponent<ObjectReset>().OriginalColor.r, objectRenderer.GetComponent<ObjectReset>().OriginalColor.g, objectRenderer.GetComponent<ObjectReset>().OriginalColor.b, SeethroughAlpha);
        //
        //            }
        //        }
        //    }
        //break;
        //
        //case HighlightType.HandHighlight:
        //    {
        //        if (manipulationEventData.Pointer != null)
        //        {
        //            if (manipulationEventData.Pointer.Controller != null)
        //            {
        //                // If controller is of kind IMixedRealityHand, return average of index and thumb
        //                if (manipulationEventData.Pointer.Controller is IMixedRealityHand hand)
        //                {
        //                    if (hand.Visualizer != null)
        //                    {
        //                        grabbableObject = manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject;
        //                        handRenderer = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>();
        //
        //                        Vector4 grabbableObjectPos = hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().worldToLocalMatrix * manipulationEventData.ManipulationSource.GetComponentInChildren<Renderer>().gameObject.transform.position;
        //                        hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetVector(GrabbableObjectPosPropertyName, grabbableObjectPos);
        //                        Debug.Log(grabbableObjectPos);
        //                        hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat(HighlightHandPropertyName, 1.0f);
        //                        //hand.Visualizer.GameObjectProxy.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor(colourPropertyName, contactColour);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //break;
        //
        //default:
        //break;
        //}
    }
}
