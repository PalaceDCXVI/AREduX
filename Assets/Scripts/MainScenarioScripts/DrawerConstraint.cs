using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerConstraint : MonoBehaviour
{
    public float minDistance = 0;
    public float maxDistance = 0;

    public float startLocation = 0;
    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > startLocation + maxDistance)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startLocation + maxDistance /*transform.position.z*/);
        }

        if (transform.position.z < startLocation - minDistance)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startLocation - minDistance /*transform.position.z*/);
        }
    }
}
