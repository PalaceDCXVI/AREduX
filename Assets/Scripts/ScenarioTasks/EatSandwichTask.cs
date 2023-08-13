using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatSandwichTask : ScenarioTask
{
    private void OnTriggerEnter(Collider other)
    {
        if (hasStarted && other.CompareTag("Breadslice") && other.GetComponent<ManipulationCheck>() && other.GetComponent<ManipulationCheck>().isBeingManipulated)
        {
            CompleteTask();
        }
    }
}
