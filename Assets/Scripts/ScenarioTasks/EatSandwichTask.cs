using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatSandwichTask : ScenarioTask
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
        if (hasStarted && other.CompareTag("Breadslice") && other.GetComponent<ManipulationCheck>() && other.GetComponent<ManipulationCheck>().isBeingManipulated)
        {
            CompleteTask();
        }
    }
}
