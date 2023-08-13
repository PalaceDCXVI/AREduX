using Microsoft.MixedReality.OpenXR;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPlacementTagComparison : MonoBehaviour
{
    public string TagToCompare = "";

    public AudioSource soundFXPlayer;
    public AudioClip placementSound;

    public bool PermanentPlacement = true;

    public UnityEvent PlacementEvents;

    // Start is called before the first frame update
    void Start()
    {
        if (TagToCompare == "")
        {
            Debug.LogError("ObjectPlacementScript does not have a tag provided!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlaceObject(Collider other)
    {
        soundFXPlayer.PlayOneShot(placementSound);

        other.gameObject.GetComponent<ObjectManipulator>().ForceEndManipulation();

        other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (PermanentPlacement)
        {
            other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            other.transform.SetParent(this.transform);
            other.gameObject.transform.localPosition = Vector3.zero;
            other.gameObject.transform.localRotation = Quaternion.identity;
        }
        else
        {
            other.gameObject.transform.position = this.transform.position;
            other.gameObject.transform.rotation = this.transform.rotation;
        }

        //Should really seperate this out, such that tasks that fire in response to placement are placed in the events event but uh... We'll see if that ever happens. Gonna bet on no.
        GetComponentInParent<PlaceUtencilTask>()?.PlaceObject(other.gameObject);
        
        PlacementEvents.Invoke();

        foreach (var objectRenderer in other.gameObject.GetComponentsInChildren<Renderer>())
        {
            objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
        }

        if (TagToCompare.Contains("plate"))
        {
            Vector3 fullCenter = new Vector3(-1.52736902e-07f, -0.0149999997f, 1.11758709e-07f);
            Vector3 fullSize = new Vector3(0.169024125f, 0.0599999987f, 0.16902411f);
            other.gameObject.GetComponent<BoxCollider>().center = fullCenter;
            other.gameObject.GetComponent<BoxCollider>().size = fullSize;
        }
        
        other.gameObject.GetComponent<ManipulationCheck>().canBeSlotted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower().Contains(TagToCompare) && other.gameObject.GetComponent<ManipulationCheck>() && other.gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
        {
            PlaceObject(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.ToLower().Contains(TagToCompare) && other.gameObject.GetComponent<ManipulationCheck>() && other.gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
        {
            PlaceObject(other);
        }
    }
}
