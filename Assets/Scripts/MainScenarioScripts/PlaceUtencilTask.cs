using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUtencilTask : ScenarioTask
{
    public int correctNumberofUtencils = 0;
    public List<GameObject> collidedObjects;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartTask()
    {
        base.StartTask();

        correctNumberofUtencils = GameObject.FindGameObjectsWithTag("Utencil").Length;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasStarted)
        {
            if (collision.gameObject.CompareTag("Utencil"))
            {
                collidedObjects.Add(collision.gameObject);
            }
            if (collidedObjects.Count == correctNumberofUtencils && !hasEnded)
            {
                CompleteTask();
            }
        }

        
    }

    void OnCollisionExit(Collision collision)
    {
        if (hasStarted)
        {
            if (collision.gameObject.CompareTag("Utencil"))
            {
                collidedObjects.Remove(collision.gameObject);
            }
        }
    }

}
