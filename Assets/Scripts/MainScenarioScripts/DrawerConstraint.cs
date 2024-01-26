using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerConstraint : MonoBehaviour
{
    public float minDistance = 0;
    public float maxDistance = 0;

    public Vector3 startLocation = Vector3.zero;

    Quaternion startingLocalRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.localPosition;
        startingLocalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x > startLocation.x + maxDistance)
        {
            transform.localPosition = new Vector3(startLocation.x - minDistance, transform.localPosition.y, transform.localPosition.z /*transform.position.z*/);
        }

        if (transform.localPosition.x < startLocation.x - minDistance)
        {
            transform.localPosition = new Vector3(startLocation.x - minDistance, transform.localPosition.y, transform.localPosition.z  /*transform.position.z*/);
        }

        //transform.localRotation= startingLocalRotation;
    }
}
