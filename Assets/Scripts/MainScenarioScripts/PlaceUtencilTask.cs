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
        correctNumberofUtencils = GameObject.FindGameObjectsWithTag("Utencil").Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Utencil"))
        {
            collidedObjects.Add(collision.gameObject);
        }
        if (collidedObjects.Count == correctNumberofUtencils)
        {
            CompleteTask();
        }
        
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Utencil"))
        {
            collidedObjects.Remove(collision.gameObject);
        }
    }

}
