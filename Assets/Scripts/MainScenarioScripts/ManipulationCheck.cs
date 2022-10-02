using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationCheck : MonoBehaviour
{
    private ObjectManipulator manipulator;

    public bool isBeingManipulated = false;

    public bool manipulationJustEnded = false;

    public bool CanBeSlotted = false;

    public bool CanLandFirst = false;

    // Start is called before the first frame update
    void Start()
    {
        manipulator = GetComponent<ObjectManipulator>();
        manipulator.OnManipulationStarted.AddListener(OnManipulationStart);
        manipulator.OnManipulationEnded.AddListener(OnManipulationEnd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsBeingManipulated()
    {
        return CanBeSlotted;
    }

    private void OnManipulationStart(ManipulationEventData arg0)
    {
        isBeingManipulated = true;
        CanBeSlotted = false;
    }

    private void OnManipulationEnd(ManipulationEventData arg0)
    {
        isBeingManipulated = false;

        if (!CanLandFirst)
        {
            CanBeSlotted = true;
        }
        else
        {
            CanBeSlotted = true;
            manipulationJustEnded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (CanBeSlotted == true)
        {
            CanBeSlotted = false;
        }

        if (manipulationJustEnded)
        {
            manipulationJustEnded = false;
        }
    }
}
