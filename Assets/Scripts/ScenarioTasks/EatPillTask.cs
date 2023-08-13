using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPillTask : ScenarioTask
{

    private void OnTriggerEnter(Collider other)
    {
        if (hasStarted && other.CompareTag("MainCamera") && GetComponent<ManipulationCheck>() && GetComponent<ManipulationCheck>().isBeingManipulated)
        {
            CompleteTask();
        }
    }
}
