using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine;

public class ObjectReset : MonoBehaviour
{
    public Transform ResetTransform;

    private Vector3 startingPosition = new Vector3();
    private Quaternion startingRotation = new Quaternion();

    public Color OriginalColor;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;

        OriginalColor = GetComponentInChildren<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetObject()
    {
        if (ResetTransform == null)
        {
            this.transform.SetPositionAndRotation(startingPosition, startingRotation);
        }
        else 
        {
            this.transform.SetPositionAndRotation(ResetTransform.position, ResetTransform.rotation);
        }
        
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
