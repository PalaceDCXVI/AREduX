using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationCheck : MonoBehaviour
{
    private ObjectManipulator manipulator;

    public bool isBeingManipulated = false;
    public bool canBeSlotted = false;

    public float TimeDelayForSlotting = 1.0f;
    private float currentDelay = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentDelay = TimeDelayForSlotting;

        manipulator = GetComponent<ObjectManipulator>();
        manipulator.OnManipulationStarted.AddListener(OnManipulationStart);
        manipulator.OnManipulationEnded.AddListener(OnManipulationEnd);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDelay >= 0.0f && !isBeingManipulated)
        {
            currentDelay -= Time.deltaTime;
        }
        else if (currentDelay < 0.0f && !isBeingManipulated)
        {
            canBeSlotted = false;
        }
    }

    public bool CanBeSlotted()
    {
        return canBeSlotted;
    }

    private void OnManipulationStart(ManipulationEventData arg0)
    {
        isBeingManipulated = true;
        canBeSlotted = true;

        currentDelay = TimeDelayForSlotting;
    }

    private void OnManipulationEnd(ManipulationEventData arg0)
    {
        isBeingManipulated = false;
        canBeSlotted = true;

        currentDelay = TimeDelayForSlotting;
    }
}
