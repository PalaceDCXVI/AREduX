using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPillTask : ScenarioTask
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasStarted && other.CompareTag("MainCamera") && GetComponent<ManipulationCheck>() && GetComponent<ManipulationCheck>().isBeingManipulated)
        {
            CompleteTask();
        }
    }
}
