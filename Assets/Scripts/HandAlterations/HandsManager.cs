using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.XR;

public class HandsManager : MonoBehaviour
{
    [SerializeField]
    private TrackedHandJoint trackedHandJoint = TrackedHandJoint.IndexMiddleJoint;

    private IMixedRealityHandJointService handJointService;

    private IMixedRealityHandJointService HandJointService =>
        handJointService ??
        (handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

    private MixedRealityPose? previousLeftHandPose;
    private bool leftHandIsGrabbing = false;
    private bool previousLeftHandIsGrabbing = false;

    private MixedRealityPose? previousRightHandPose;
    private bool rightHandIsGrabbing = false;
    private bool previousRightHandIsGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var leftHandPose = GetHandPose(Handedness.Left, previousLeftHandPose != null);
        leftHandIsGrabbing = CheckHandIsGrabbing(Handedness.Left);
        if (!previousLeftHandIsGrabbing && leftHandIsGrabbing)
        {
            if (Physics.CheckSphere(HandJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left).position, SphereCursor.KnownSphereCastRadius * 2.0f))
            {
                Debug.Log("Left Hand Grab Event");
                SimulationDataManager.Instance.AddGraspAttempt();
            }
        }

        var rightHandPose = GetHandPose(Handedness.Right, previousRightHandPose != null);
        rightHandIsGrabbing = CheckHandIsGrabbing(Handedness.Right);
        if (!previousRightHandIsGrabbing && rightHandIsGrabbing)
        {
            if (Physics.CheckSphere(HandJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right).position, SphereCursor.KnownSphereCastRadius * 2.0f))
            {
                Debug.Log("Right Hand Grab Event");
                SimulationDataManager.Instance.AddGraspAttempt();
            }
        }

        previousLeftHandPose = leftHandPose;
        previousLeftHandIsGrabbing = leftHandIsGrabbing;
        previousRightHandPose = rightHandPose;
        previousRightHandIsGrabbing = rightHandIsGrabbing;
    }

    private MixedRealityPose? GetHandPose(Handedness hand, bool hasBeenGrabbed)
    {
        if (HandJointService.IsHandTracked(hand) /*&&
            ((GestureUtils.IsPinching(hand) && trackPinch) ||
                (GestureUtils.IsGrabbing(hand) && trackGrab))*/)
        {
            var jointTransform = HandJointService.RequestJointTransform(trackedHandJoint, hand);
            var palmTransForm = HandJointService.RequestJointTransform(TrackedHandJoint.Palm, hand);

            return new MixedRealityPose(jointTransform.position, palmTransForm.rotation);
        }

        return null;
    }

    private bool CheckHandIsGrabbing(Handedness hand)
    {
        return HandPoseUtils.IsThumbGrabbing(hand) || HandPoseUtils.IsIndexGrabbing(hand) || HandPoseUtils.IsMiddleGrabbing(hand);
    }

}
