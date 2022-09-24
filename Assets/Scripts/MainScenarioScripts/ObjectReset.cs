using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReset : MonoBehaviour
{
    private Vector3 startingPosition = new Vector3();
    private Quaternion startingRotation = new Quaternion();
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetObject()
    {
        this.transform.SetPositionAndRotation(startingPosition, startingRotation);
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }


    }
}
