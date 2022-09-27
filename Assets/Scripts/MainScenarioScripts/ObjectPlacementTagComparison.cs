using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementTagComparison : MonoBehaviour
{
    public string TagToCompare = "";
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower().Contains(TagToCompare))
        {
            other.transform.SetParent(this.transform);

            other.gameObject.GetComponent<ObjectManipulator>().ForceEndManipulation();
            other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            other.gameObject.transform.localPosition = Vector3.zero;
            other.gameObject.transform.localRotation = Quaternion.identity;

            GetComponentInParent<PlaceUtencilTask>().PlaceObject(other.gameObject);
        }
    }
}
