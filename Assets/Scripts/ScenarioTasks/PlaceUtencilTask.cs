using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PlaceUtencilTask : ScenarioTask
{
    public int correctNumberofUtencils = 0;
    public List<GameObject> collidedObjects;

    public GameObject ForkSlot;
    public GameObject KnifeSlot;
    public GameObject SpoonSlot;
    public GameObject PlateSlot;
    public GameObject GlassSlot;

    public AudioSource soundFXPlayer;
    public AudioClip incorrectPlacementSound;

    public override void StartTask()
    {
        base.StartTask();

        if (correctNumberofUtencils == 0)
        {
            Debug.LogError("Number of Utensils has not been set!!!");
        }
    }

    public void PlaceObject(GameObject gameObject)
    {
        if (!collidedObjects.Contains(gameObject))
        {
            collidedObjects.Add(gameObject);

            gameObject.GetComponent<ObjectManipulator>().ForceEndManipulation();
            gameObject.GetComponent<ObjectManipulator>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
        }

        Debug.Log("collidedObjects: " + collidedObjects.Count);
        Debug.Log("Place Utencils Has Ended: " + hasEnded);

        if (collidedObjects.Count == correctNumberofUtencils && !hasEnded)
        {
            CompleteTask();
        }
    }

    public override void ResetScenario()
    {
        base.ResetScenario();
        collidedObjects.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
         if (hasStarted)
         { 
            if ((other.gameObject.CompareTag("fork") || other.gameObject.CompareTag("knife") || other.gameObject.CompareTag("spoon") || other.gameObject.CompareTag("plate") || other.gameObject.CompareTag("glass")
                && other.gameObject.GetComponent<ObjectManipulator>().enabled) )
            {
                soundFXPlayer.PlayOneShot(incorrectPlacementSound);
            }
         }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (hasStarted)
    //    {
    //        if (collision.gameObject.CompareTag("Utencil") && !collidedObjects.Contains(collision.gameObject))
    //        {
    //            collidedObjects.Add(collision.gameObject);
    //
    //            if (collision.gameObject.name.ToLower().Contains("fork"))
    //            {
    //                collision.transform.SetParent(ForkSlot.transform);
    //            }
    //            else if (collision.gameObject.name.ToLower().Contains("knife"))
    //            {
    //                collision.transform.SetParent(KnifeSlot.transform);
    //            }
    //            else if (collision.gameObject.name.ToLower().Contains("spoon"))
    //            {
    //                collision.transform.SetParent(SpoonSlot.transform);
    //            }
    //            else if (collision.gameObject.name.ToLower().Contains("plate"))
    //            {
    //                collision.transform.SetParent(PlateSlot.transform);
    //            }
    //            else if (collision.gameObject.name.ToLower().Contains("glass"))
    //            {
    //                collision.transform.SetParent(GlassSlot.transform);
    //            }
    //
    //            collision.gameObject.GetComponent<ObjectManipulator>().ForceEndManipulation();
    //            collision.gameObject.GetComponent<ObjectManipulator>().enabled = false;
    //            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    //            collision.transform.localPosition = Vector3.zero;
    //            collision.transform.localRotation = Quaternion.identity;
    //        }
    //
    //        if (collidedObjects.Count == correctNumberofUtencils && !hasEnded)
    //        {
    //            CompleteTask();
    //        }
    //    }
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    if (hasStarted)
    //    {
    //        if (collision.gameObject.CompareTag("Utencil") && collidedObjects.Contains(collision.gameObject))
    //        {
    //            collidedObjects.Remove(collision.gameObject);
    //        }
    //    }
    //}

}
