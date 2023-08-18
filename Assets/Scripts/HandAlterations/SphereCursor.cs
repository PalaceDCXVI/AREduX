using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Physics;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class SphereCursor : MonoBehaviour
{
    SpherePointer spherePointer = null; 
    GameObject sphere = null;
    public Material sphereMaterial = null;
    public Material dotMaterial = null;
    // Start is called before the first frame update
    void Start()
    {
        spherePointer = GetComponent<SpherePointer>();
        
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        float scale = spherePointer.SphereCastRadius;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.transform.SetParent(this.transform, false);
        sphere.GetComponent<Renderer>().enabled = true;
        sphere.GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position;
        float scale = spherePointer.SphereCastRadius;

        switch (MaterialManager.highlightType)
        {
            case MaterialManager.HighlightType.SphericalCursor:
            sphere.SetActive(true);
            sphere.GetComponent<Renderer>().enabled = true;
            sphere.GetComponent<Renderer>().material = sphereMaterial;
            TryGetNearGraspPoint(out position);
            sphere.transform.position = position;
            sphere.transform.localScale = new Vector3(scale, scale, scale);
            break;    

            case MaterialManager.HighlightType.DotCursor:
            sphere.SetActive(true);
            sphere.GetComponent<Renderer>().enabled = true;
            sphere.GetComponent<Renderer>().material = dotMaterial;
            TryGetNearGraspPoint(out position);
            sphere.transform.position = position;
            scale = spherePointer.SphereCastRadius * 0.1f;
            sphere.transform.localScale = new Vector3(scale, scale, scale);
            break;   
            
            default:
            sphere.SetActive(false);
            sphere.GetComponent<Renderer>().enabled = false;
            sphere.GetComponent<Renderer>().material = sphereMaterial;
            break;
        } 
    }


    public bool TryGetNearGraspPoint(out Vector3 result)
    {      
        if (spherePointer.Controller != null)
        {
            // If controller is of kind IMixedRealityHand, return average of index and thumb
            if (spherePointer.Controller is IMixedRealityHand hand)
            {
                if (hand.TryGetJoint(TrackedHandJoint.IndexTip, out MixedRealityPose index) && index != null)
                {
                    //case GraspPointPlacement.BetweenIndexFingerAndThumb:
                    if (hand.TryGetJoint(TrackedHandJoint.ThumbTip, out MixedRealityPose thumb) && thumb != null)
                    {
                    result = 0.5f * (index.Position + thumb.Position);
                    return true;
                    }
                }
            }
    
            // If controller isn't an IMixedRealityHand or one of the required joints isn't available, check for position
            //if (spherePointer.Controller.IsPositionAvailable)
            //{
            //    result = Position;
            //    return true;
            //}
        }

        result = Vector3.zero;
        return false;
    }

    /// <summary>
    /// When in editor, draws an approximation of what is the "Near Object" area
    /// </summary>
    private void DrawSphereCursor()
    {

        //if (!IsActive)
        //    return;
//
        //bool NearObjectCheck = queryBufferNearObjectRadius != null && IsNearObject;
        //bool IsInteractionEnabledCheck = queryBufferInteractionRadius != null && IsInteractionEnabled;
//
        //TryGetNearGraspAxis(out Vector3 sectorForwardAxis);
        //TryGetNearGraspPoint(out Vector3 point);
        //Vector3 centralAxis = sectorForwardAxis.normalized;
//
        //float gizmoNearObjectRadius;
        //if (NearObjectCheck)
        //    gizmoNearObjectRadius = NearObjectRadius * (1 + nearObjectSmoothingFactor);
        //else
        //    gizmoNearObjectRadius = NearObjectRadius;
//
//
        //if (NearObjectSectorAngle >= 360.0f)
        //{
        //    // Draw the sphere and the inner near interaction deadzone (governed by the pullback distance)
        //    Gizmos.color = (NearObjectCheck ? Color.red : Color.cyan) - Color.black * 0.8f;
        //    Gizmos.DrawSphere(point - centralAxis * PullbackDistance, gizmoNearObjectRadius);
//
        //    Gizmos.color = Color.blue - Color.black * 0.8f;
        //    Gizmos.DrawSphere(point - centralAxis * PullbackDistance, PullbackDistance);
        //}
        //else
        //{
        //    // Draw something approximating the sphere's sector
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(point, point + centralAxis * (gizmoNearObjectRadius - PullbackDistance));
//
        //    UnityEditor.Handles.color = NearObjectCheck ? Color.red : Color.cyan;
        //    float GizmoAngle = NearObjectSectorAngle * 0.5f * Mathf.Deg2Rad;
        //    UnityEditor.Handles.DrawWireDisc(point,
        //                                        centralAxis,
        //                                        PullbackDistance * Mathf.Sin(GizmoAngle));
//
        //    UnityEditor.Handles.DrawWireDisc(point + sectorForwardAxis.normalized * (gizmoNearObjectRadius * Mathf.Cos(GizmoAngle) - PullbackDistance),
        //                                        centralAxis,
        //                                        gizmoNearObjectRadius * Mathf.Sin(GizmoAngle));
        //}
//
        //// Draw the sphere representing the grabbable area
        //Gizmos.color = Color.green - Color.black * (IsInteractionEnabledCheck ? 0.3f : 0.8f);
        //Gizmos.DrawSphere(point, SphereCastRadius);
    }
}
